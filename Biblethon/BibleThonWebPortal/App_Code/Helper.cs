using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.IO;
using Microsoft.Dynamics.GP.eConnect;
using System.Xml;
using Microsoft.Dynamics.GP.eConnect.Serialization;
using System.Xml.Serialization;

/// <summary>
/// Summary description for Helper
/// </summary>
public static class Helper
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);    

    public static bool IsDocumentForRevision(Dictionary<string, string> pDic)
    {
        bool rtn = false;

        using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AWFConnectionString"].ConnectionString))
        {
            try
            {
                // Save First
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                SqlCommand command = connection.CreateCommand();

                string commandText = "SELECT COUNT(*) FROM [AS_WF_WorkflowInstance] WHERE WFRequestType = 'Revision' AND WFStatus = 'Pending Action' ";

                foreach (KeyValuePair<string, string> kvp in pDic)
                {
                    commandText += "AND " + kvp.Key + " = @" + kvp.Key;
                    command.Parameters.AddWithValue("@" + kvp.Key, kvp.Value);
                }

                command.CommandText = commandText;                

                object obj = command.ExecuteScalar();
                if (obj != null && Convert.ToInt32(obj) > 0)
                    rtn = true;
            }
            catch (Exception ex)
            {
                //Insert Logger Here
            }
            finally
            {
                connection.Close();
            }

            return rtn;
        }
    }

    public static List<string> RetrieveShippingMethods()
    {
        List<string> rtn = new List<string>();

   //     using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString))
        using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GPConnectionString"].ConnectionString))
        {
            try
            {
                // Save First
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                SqlCommand command = connection.CreateCommand();

                string commandText = "SELECT ShippingMethodID FROM [AS_Alere_ShippingMethods] ";                

                command.CommandText = commandText;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    rtn.Add(reader[0].ToString());
                }
            }
            catch (Exception ex)
            {
                //Insert Logger Here
            }
            finally
            {
                connection.Close();
            }

            return rtn;
        }
    }

    public static string GetNextGPNumber()
    {
        string rtn = string.Empty;

        using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GPConnectionString"].ConnectionString))
        {
            try
            {
                // Save First
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                SqlCommand command = connection.CreateCommand();

                string commandText = "sopGetIDNumber";
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = commandText;

                command.Parameters.AddWithValue("@I_tSOPTYPE", 2); // ORDER
                command.Parameters.AddWithValue("@I_cDocumentID", ConfigurationManager.AppSettings["OrderDocumentID"]); // From Web.Config
                command.Parameters.AddWithValue("@I_tInc_Dec", 1);

                SqlParameter outputIdParam = new SqlParameter("@O_vSOPNumber", SqlDbType.VarChar, 21)
                {
                    Direction = ParameterDirection.Output
                };

                command.Parameters.Add(outputIdParam);

                command.ExecuteNonQuery();

                rtn = outputIdParam.Value.ToString().Trim();
            }
            catch (Exception ex)
            {
                //Insert Logger Here
            }
            finally
            {
                connection.Close();
            }

            return rtn;
        }
    }

    public static string GetNextSequenceNumber(string pDocumentType)
    {
        string rtn = string.Empty;

        WorkflowDataContext wfDataContext = new WorkflowDataContext();

        wfDataContext.AS_WF_spGetNextInSequence(pDocumentType, ref rtn);

        return rtn;
    }

    public static List<string> UserGroup(string pUserID)
    {
        List<string> rtn = new List<string>();

        using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AWFConnectionString"].ConnectionString))
        {
            try
            {
                // Save First
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                SqlCommand command = connection.CreateCommand();

                string commandText = "SELECT [GroupID] FROM [AS_WF_UserGroups] WHERE [UserID] = @pUserID ";

                command.CommandText = commandText;
                command.Parameters.AddWithValue("@pUserID", pUserID);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    rtn.Add(reader[0].ToString());
                }
            }
            catch (Exception ex)
            {
                //Insert Logger Here
            }
            finally
            {
                connection.Close();
            }

            return rtn;
        }
    }

    public static GPItem RetrieveGPItemObj(string pItemSKU, SqlConnection pConn)
    {
        GPItem rtn = new GPItem();
        string sql = "SELECT ITEMDESC, ITMSHNAM FROM [IV00101] WHERE ([ITEMNMBR] = @pItemSKU)";
        
        SqlCommand cmd = new SqlCommand(sql, pConn);

        cmd.Parameters.AddWithValue("@pItemSKU", pItemSKU);

        try
        {
            if (pConn.State == ConnectionState.Closed)
                pConn.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
	            rtn.ITEMDESC = reader["ITEMDESC"].ToString();
                rtn.ITMSHNAM = reader["ITMSHNAM"].ToString();
            }
            reader.Close();
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }        

        return rtn;

    }

    public static string RetrieveItemDescription(string pItemSKU)
    {
        string rtn = string.Empty;
        string sql = "SELECT ITEMDESC FROM [IV00101] WHERE ([ITEMNMBR] = @pItemSKU)";

        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@pItemSKU", pItemSKU);

        try
        {
            conn.Open();

            object obj = cmd.ExecuteScalar();
            if (obj != null)
            {
                rtn = obj.ToString();
            }
        }
        catch (Exception ex)
        {

        }
        finally
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }

        return rtn;

    }

    public static string RetrieveItemShortName(string pItemSKU)
    {
        string rtn = string.Empty;
        string sql = "SELECT ITMSHNAM FROM [IV00101] WHERE ([ITEMNMBR] = @pItemSKU)";

        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@pItemSKU", pItemSKU);

        try
        {
            conn.Open();

            object obj = cmd.ExecuteScalar();
            if (obj != null)
            {
                rtn = obj.ToString();
            }
        }
        catch (Exception ex)
        {

        }
        finally
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }

        return rtn;

    }

    public static string RetrieveExtenderStudioField(string pItemSKU, SqlConnection pConn)
    {
        string rtn = string.Empty;
        string sql = "SELECT eic.[Studio / Licensor] FROM dbo.extender_item_classification eic WHERE eic.[Item Number] = @pItemSKU";
        
        SqlCommand cmd = new SqlCommand(sql, pConn);

        cmd.Parameters.AddWithValue("@pItemSKU", pItemSKU);

        try
        {
            if (pConn.State == ConnectionState.Closed)
            {
                pConn.Open();
            }            

            object obj = cmd.ExecuteScalar();
            if (obj != null)
            {
                rtn = obj.ToString();
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }       

        return rtn;

    }

    public static string RetrieveItemSKUFromShortName(string pItemShortName, SqlConnection pConn)
    {
        string rtn = string.Empty;
        string sql = "SELECT ITEMNMBR FROM [IV00101] WHERE ([ITMSHNAM] = @pItemShortName)";
        
        SqlCommand cmd = new SqlCommand(sql, pConn);

        cmd.Parameters.AddWithValue("@pItemShortName", pItemShortName);

        try
        {
            if (pConn.State == ConnectionState.Closed)
            {
                pConn.Open();
            }            

            object obj = cmd.ExecuteScalar();
            if (obj != null)
            {
                rtn = obj.ToString();
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        
        return rtn;

    }

    public static List<string> RetrieveCustomerIDs()
    {
        List<string> rtn = new List<string>();

        using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString))
        {
            try
            {
                // Save First
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                SqlCommand command = connection.CreateCommand();

                string commandText = "SELECT TOP 500 CUSTNMBR FROM RM00101 WHERE CUSTCLAS NOT LIKE 'SPRI%' AND CUSTCLAS <> 'MUNIC/MLTY' AND CUSTCLAS <> 'TRADESHOWS' ORDER BY CUSTNMBR";

                command.CommandText = commandText;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    rtn.Add(reader[0].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                //Insert Logger Here
            }
            finally
            {
                connection.Close();
            }

            return rtn;
        }
    }

    public static List<string> RetrieveCustomerNames()
    {
        List<string> rtn = new List<string>();

        using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString))
        {
            try
            {
                // Save First
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                SqlCommand command = connection.CreateCommand();

                string commandText = "SELECT TOP 500 CUSTNAME FROM RM00101 WHERE CUSTCLAS NOT LIKE 'SPRI%' AND CUSTCLAS <> 'MUNIC/MLTY' AND CUSTCLAS <> 'TRADESHOWS' ORDER BY CUSTNMBR";

                command.CommandText = commandText;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    rtn.Add(reader[0].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                //Insert Logger Here
            }
            finally
            {
                connection.Close();
            }

            return rtn;
        }
    }

    public static GPCustomer RetrieveCustomerData(string pCustomerNumber)
    {
        GPCustomer rtn = new GPCustomer();
        string sql = "SELECT CUSTNAME, CUSTCLAS, ADDRESS1, ADDRESS2, ADDRESS3, CITY, COUNTRY, STATE, ZIP FROM RM00101 WHERE CUSTNMBR = @pCustomerNumber";

        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@pCustomerNumber", pCustomerNumber);

        try
        {
            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                rtn = new GPCustomer()
                {
                    CUSTNAME = reader["CUSTNAME"].ToString(),
                    CUSTCLAS = reader["CUSTCLAS"].ToString(),
                    ADDRESS1 = reader["ADDRESS1"].ToString(),
                    ADDRESS2 = reader["ADDRESS2"].ToString(),
                    ADDRESS3 = reader["ADDRESS3"].ToString(),
                    CITY = reader["CITY"].ToString(),
                    COUNTRY = reader["COUNTRY"].ToString(),
                    STATE = reader["STATE"].ToString(),
                    ZIP = reader["ZIP"].ToString()
                };
            }
        }
        catch (Exception ex)
        {

        }
        finally
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }

        return rtn;

    }

    public static bool GAIAM_InsertDetailTemp(string pAuthorizationNumber, string pUPCCode, string pGAIAMItemNumber, int pQuantity, decimal pAmountPerUnit, decimal pTotalAmount, SqlConnection pConn)
    {
        bool rtn = false;
        string sql = "[AS_WF_spInsertIntoGaiamChargebackDetailTemp]";

        SqlCommand cmd = new SqlCommand(sql, pConn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@pAuthorizationNumber", pAuthorizationNumber);
        cmd.Parameters.AddWithValue("@pUPCCode", pUPCCode);
        cmd.Parameters.AddWithValue("@pGAIAMItemNumber", pGAIAMItemNumber);
        cmd.Parameters.AddWithValue("@pQuantity", pQuantity);
        cmd.Parameters.AddWithValue("@pAmountPerUnit", pAmountPerUnit);
        cmd.Parameters.AddWithValue("@pTotalAmount", pTotalAmount);

        try
        {
            if (pConn.State == ConnectionState.Closed)
            {
                pConn.Open();
            }

            cmd.ExecuteNonQuery();
            rtn = true;
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        return rtn;
    }

    public static bool GAIAM_ProcessDetailTemp(string pAuthorizationNumber, SqlConnection pConn)
    {
        bool rtn = false;
        string sql = "[AS_WF_spProcessGaiamChargebackDetailTemp]";

        SqlCommand cmd = new SqlCommand(sql, pConn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 500;

        cmd.Parameters.AddWithValue("@pAuthorizationNumber", pAuthorizationNumber);        

        try
        {
            if (pConn.State == ConnectionState.Closed)
            {
                pConn.Open();
            }

            cmd.ExecuteNonQuery();
            rtn = true;
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        return rtn;
    }

    public static bool GAIAM_DeleteDetailTemp(string pAuthorizationNumber, SqlConnection pConn)
    {
        bool rtn = false;
        string sql = "DELETE FROM AS_GAIAM_Chargeback_Detail_Temp WHERE [AuthorizationNumber] = @pAuthorizationNumber";

        SqlCommand cmd = new SqlCommand(sql, pConn);
        cmd.CommandType = CommandType.Text;

        cmd.Parameters.AddWithValue("@pAuthorizationNumber", pAuthorizationNumber);
        
        try
        {
            if (pConn.State == ConnectionState.Closed)
            {
                pConn.Open();
            }

            cmd.ExecuteNonQuery();
            rtn = true;
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        return rtn;
    }

    public static string RetrieveVendorName(string pVendorID)
    {
        string rtn = string.Empty;
        string sql = "SELECT VENDNAME FROM [PM00200] WHERE ([VENDORID] = @pVendorID)";

        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@pVendorID", pVendorID);

        try
        {
            conn.Open();

            object obj = cmd.ExecuteScalar();
            if (obj != null)
            {
                rtn = obj.ToString();
            }
        }
        catch (Exception ex)
        {

        }
        finally
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }

        return rtn;

    }

    public static string RetrieveCustomerIDFromCustomerName(string pCustomerName)
    {
        string rtn = string.Empty;
        string sql = "SELECT CUSTNMBR FROM [RM00101] WHERE ([CUSTNAME] = @pCustomerName)";

        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@pCustomerName", pCustomerName);

        try
        {
            conn.Open();

            object obj = cmd.ExecuteScalar();
            if (obj != null)
            {
                rtn = obj.ToString();
            }
        }
        catch (Exception ex)
        {

        }
        finally
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }

        return rtn;

    }

    public static bool IsItemSKUValid(string pItemSKU, SqlConnection pConn)
    {
        bool rtn = false;
        string sql = "SELECT ITEMNMBR FROM [IV00101] WHERE ([ITEMNMBR] = @pItemSKU)";

        SqlCommand cmd = new SqlCommand(sql, pConn);

        cmd.Parameters.AddWithValue("@pItemSKU", pItemSKU);

        try
        {
            if(pConn.State == ConnectionState.Closed)
                pConn.Open();

            object obj = cmd.ExecuteScalar();
            if (obj != null)
                rtn = true;
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }       

        return rtn;

    }

    public static bool IsItemShortNameValid(string pItemSKU, string pItemShortName, SqlConnection pConn)
    {
        bool rtn = false;
        string sql = "SELECT ITEMNMBR FROM [IV00101] WHERE ([ITEMNMBR] = @pItemSKU) AND ([ITMSHNAM] = @pItemShortName)";


        SqlCommand cmd = new SqlCommand(sql, pConn);

        cmd.Parameters.AddWithValue("@pItemSKU", pItemSKU);
        cmd.Parameters.AddWithValue("@pItemShortName", pItemShortName);

        try
        {
            if (pConn.State == ConnectionState.Closed)
                pConn.Open();

            object obj = cmd.ExecuteScalar();
            if (obj != null)
                rtn = true;
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        

        return rtn;

    }

    public static bool IsCustomerNumberValid(string pCustomerNumber)
    {
        bool rtn = false;
        string sql = "SELECT CUSTNMBR FROM [RM00101] WHERE ([CUSTNMBR] = @pCustomerNumber)";

        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@pCustomerNumber", pCustomerNumber);

        try
        {
            conn.Open();

            object obj = cmd.ExecuteScalar();
            if (obj != null)
                rtn = true;
        }
        catch (Exception ex)
        {

        }
        finally
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }

        return rtn;

    }

    public static bool IsSalesPersonValid(string pSalesPersonID)
    {
        bool rtn = false;
        string sql = "SELECT SLPRSNID FROM RM00301 WHERE ([SLPRSNID] = @pSLPRSNID)";

        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@pSLPRSNID", pSalesPersonID);

        try
        {
            conn.Open();

            object obj = cmd.ExecuteScalar();
            if (obj != null)
                rtn = true;
        }
        catch (Exception ex)
        {

        }
        finally
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }

        return rtn;

    }

    public static WorkflowUser RetrieveUserData(string pUserID)
    {
        WorkflowUser rtn = new WorkflowUser();
        string sql = "SELECT [UserPassword], [ERPUserID], [FirstName], [LastName], [Department], [Company], " +
            "[Phone], [EmailAdd], [UserRole], [CompanyPosition] FROM [AWF].[dbo].[AS_WF_Users] WHERE ([UserID] = @pUserID)";

        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AWFConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@pUserID", pUserID);

        try
        {
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                rtn = new WorkflowUser()
                {
                    UserID = pUserID,
                    ERPUserID = reader["ERPUserID"].ToString(),
                    FirstName = reader["FirstName"].ToString(),
                    LastName = reader["LastName"].ToString(),
                    Department = reader["Department"].ToString(),
                    Company = reader["Company"].ToString(),
                    EmailAdd = reader["EmailAdd"].ToString(),
                    UserRole = reader["UserRole"].ToString()

                };
            }
        }
        catch (Exception ex)
        {

        }
        finally
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }

        return rtn;

    }

    public static string RetrieveWFCommentFromDocKeyString(string pDocKey_Str1)
    {
        string rtn = string.Empty;

        string sql = "SELECT [Comment] FROM [AWF].[dbo].[AS_WF_WorkflowInstance] WHERE ([DocKey_Str1] = @pDocKey_Str1)";

        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AWFConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@pDocKey_Str1", pDocKey_Str1);

        try
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            object obj = cmd.ExecuteScalar();

            if (obj != null)
                rtn = obj.ToString();
        }
        catch (Exception ex)
        {

        }
        finally
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }

        return rtn;
    }

    /// <summary>
    /// This will check if the user is part of a group that has "Admin" access to a specific workflow document type. User is not necessarily a Workflow Admin
    /// </summary>
    /// <param name="pUserID">User ID to check</param>
    /// <param name="pDocType">Workflow Document Type to check</param>
    /// <returns></returns>
    public static bool IsUserEditor(string pUserID, string pDocType)
    {
        bool rtn = false;

        string sql = "SELECT COUNT(*) FROM AS_WF_UserGroups LEFT OUTER JOIN AS_WF_GroupsWorkflowDocumentTypes ON AS_WF_UserGroups.GroupID = AS_WF_GroupsWorkflowDocumentTypes.GroupID " +
            "WHERE (AS_WF_UserGroups.UserID = @pUserID and AS_WF_GroupsWorkflowDocumentTypes.DocType = @pDocType)";

        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AWFConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@pUserID", pUserID);
        cmd.Parameters.AddWithValue("@pDocType", pDocType);

        try
        {
            if(conn.State == ConnectionState.Closed)
            conn.Open();

            object obj = cmd.ExecuteScalar();
            if (Convert.ToInt32(obj) > 0)
                rtn = true;
        }
        catch (Exception ex)
        {

        }
        finally
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }

        return rtn;

    }


    #region Econnect

    private static string SerializeGLDocumentObject(StringWriter pStringWriter, GPGLDocument pGLDocument)
    {
        log.Debug("SerializeGLDocumentObject: Starting");

        try
        {
            GPGLItemList glItems = pGLDocument.GLItems;
            eConnectType eConnect = new eConnectType();
            taGLTransactionHeaderInsert glHeader = new taGLTransactionHeaderInsert();
            taGLTransactionLineInsert_ItemsTaGLTransactionLineInsert[] itemsArray = new taGLTransactionLineInsert_ItemsTaGLTransactionLineInsert[glItems.Count];

            GLTransactionType gltype = new GLTransactionType();

            XmlSerializer serializer = new XmlSerializer(eConnect.GetType());

            glHeader.BACHNUMB = pGLDocument.BatchNumber;
            glHeader.JRNENTRY = pGLDocument.JournalEntry == 0 ? Convert.ToInt32(GenerateNextGPNumber("GL")) : pGLDocument.JournalEntry;
            glHeader.REFRENCE = pGLDocument.Reference;
            glHeader.TRXDATE = pGLDocument.TrxDate == DateTime.MinValue ? DateTime.Now.ToShortDateString() : pGLDocument.TrxDate.ToShortDateString();
            //glHeader.TRXDATE = pGLDocument.TrxDate == DateTime.MinValue ? DateTime.Now.AddYears(5).ToShortDateString() : pGLDocument.TrxDate.ToShortDateString();
            glHeader.TRXTYPE = Convert.ToInt16(pGLDocument.TrxType);
            glHeader.SERIES = Convert.ToInt16(pGLDocument.Series);

            log.Debug("SerializeGLDocumentObject: glItems Count = " + glItems.Count.ToString());

            for (int i = 0; i < glItems.Count; i++)
            {
                taGLTransactionLineInsert_ItemsTaGLTransactionLineInsert glLine = new taGLTransactionLineInsert_ItemsTaGLTransactionLineInsert();
                GPGLItem glItem = (GPGLItem)glItems[i];
                glLine.BACHNUMB = pGLDocument.BatchNumber;
                glLine.JRNENTRY = glHeader.JRNENTRY;
                glLine.SQNCLINE = glItem.SqncLine == 0 ? ((i + 1) * 16384) : glItem.SqncLine;
                glLine.ACTINDX = glItem.ActIndx;
                glLine.CRDTAMNT = glItem.CreditAmount;
                glLine.DEBITAMT = glItem.DebitAmount;
                glLine.ACTNUMST = glItem.ActNumSt;
                glLine.DSCRIPTN = glItem.Description;
                glLine.DOCDATE = glHeader.TRXDATE;

                itemsArray[i] = glLine;

                log.Debug("SerializeGLDocumentObject: GLItem:" + glLine.SQNCLINE + ":" + glLine.ACTINDX.ToString() + ":" + glLine.CRDTAMNT.ToString() + ":" + glLine.DEBITAMT.ToString());
            }

            gltype.taGLTransactionHeaderInsert = glHeader;
            gltype.taGLTransactionLineInsert_Items = itemsArray;

            GLTransactionType[] myGLMaster = { gltype };
            eConnect.GLTransactionType = myGLMaster;

            serializer.Serialize(pStringWriter, eConnect);
        }
        catch (eConnectException ex2)
        {
            log.Error("SerializeGLDocumentObject: eConnectException - ", ex2);
            throw ex2;
        }
        catch (System.Data.SqlClient.SqlException ex1)
        {
            log.Error("SerializeGLDocumentObject: SqlException - ", ex1);
            throw ex1;
        }
        catch (System.Exception ex)
        {
            log.Error("SerializeGLDocumentObject: Exception - ", ex);
            throw ex;
        }

        return pStringWriter.ToString();
    }

    public static bool CreateGLDocument(GPGLDocument pGLDocument)
    {
        bool rtn = false;

        try
        {
            StringWriter stringWriter = new StringWriter();

            string inputDocument = string.Empty;


            inputDocument = SerializeGLDocumentObject(stringWriter, pGLDocument);

            if (inputDocument.Trim().Length > 0)
                rtn = EntryPoint(inputDocument);

        }
        catch (Exception ex)
        {
            log.Error("CreateSOPDocument", ex);
        }

        return rtn;
    }

    private static bool EntryPoint(string pInputDocument)
    {
        string strConnection = string.Empty;
        bool rtn = false;
        eConnectMethods e = new eConnectMethods();
        string sXsdSchema = string.Empty;

        log.Debug("EntryPoint: pInputDocument =" + pInputDocument);

        try
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(@"C:\Program Files\Common Files\microsoft shared\eConnect 10\XML Sample Documents\Incoming XSD Schemas\eConnect.xsd");
            sXsdSchema = xDoc.OuterXml;
        }
        catch (Exception ex)
        {
            log.Error("EntryPoint", ex);
        }

        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["EconnectConnString"].ConnectionString);
        string ds = builder.DataSource;
        string catalog = builder.InitialCatalog;

        strConnection = "data source=" + ds + ";initial catalog=" + catalog + ";integrated security=SSPI;persist security info=False;packet size=4096;";
        //strConnection = CompanyConnectionString;

        try
        {
            rtn = e.eConnect_EntryPoint(strConnection, EnumTypes.ConnectionStringType.SqlClient, pInputDocument, EnumTypes.SchemaValidationType.XSD, sXsdSchema);
        }
        catch (eConnectException ex1)
        {
            log.Error("EntryPoint: eConnectException", ex1);
        }
        catch (SqlException ex2)
        {
            log.Error("EntryPoint: SqlException", ex2);
        }
        catch (Exception ex)
        {
            log.Error("EntryPoint: Exception", ex);
        }
        finally
        {
            e.Dispose();
        }
        return rtn;
    }

    private static string GenerateNextGPNumber(string pDocumentType)
    {
        string rtn = string.Empty;

        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["EconnectConnString"].ConnectionString);
        string ds = builder.DataSource;
        string catalog = builder.InitialCatalog;

        string strConnection = "data source=" + ds + ";initial catalog=" + catalog + ";integrated security=SSPI;persist security info=False;packet size=4096;";

        Microsoft.Dynamics.GP.eConnect.MiscRoutines.GetNextDocNumbers nextDocNumbers = new Microsoft.Dynamics.GP.eConnect.MiscRoutines.GetNextDocNumbers();

        switch (pDocumentType)
        {
            case "GL":
                rtn = nextDocNumbers.GetNextGLJournalEntryNumber(Microsoft.Dynamics.GP.eConnect.MiscRoutines.GetNextDocNumbers.IncrementDecrement.Increment, strConnection);
                break;
            default:
                break;
        }

        return rtn;
    }

    public static void GAIAM_ReverseGLEntriesForDeleted(string pAuthorizationNumber)
    {
        GPGLDocument gpGLDocument = new GPGLDocument();
        GPGLItemList gpItemList = new GPGLItemList();        

        using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString))
        {
            try
            {
                // Save First
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT JRNENTRY, ACTINDX, CRDTAMNT, DEBITAMT FROM [AS_GAIAM_Chargeback_GLEntries] WHERE AuthorizationNumber = @pReference ORDER BY SQNCLINE";
                command.Parameters.AddWithValue("@pReference", pAuthorizationNumber);
                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet();
                da.SelectCommand = command;                

                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    gpGLDocument.Reference = pAuthorizationNumber;
                    gpGLDocument.BatchNumber = GAIAM_GenerateGLBatch();
                    
                    GPGLItem glItem = new GPGLItem();

                    glItem.ActIndx = Convert.ToInt32(ds.Tables[0].Rows[i]["ACTINDX"].ToString());
                    if (Convert.ToDecimal(ds.Tables[0].Rows[i]["CRDTAMNT"].ToString()) > 0)
                    {
                        //glItem.DebitAmount = Convert.ToDecimal(ds.Tables[0].Rows[i]["CRDTAMNT"].ToString());
                        //glItem.CreditAmount = 0;
                        glItem.ActAmount -= Convert.ToDecimal(ds.Tables[0].Rows[i]["CRDTAMNT"].ToString());
                    }
                    else
                    {
                        //glItem.CreditAmount = Convert.ToDecimal(ds.Tables[0].Rows[i]["DEBITAMT"].ToString());
                        //glItem.DebitAmount = 0;
                        glItem.ActAmount += Convert.ToDecimal(ds.Tables[0].Rows[i]["DEBITAMT"].ToString());
                    }

                    object obj = gpItemList[gpItemList.IndexOfKey(Convert.ToInt32(ds.Tables[0].Rows[i]["ACTINDX"].ToString()))];

                    // Existing record
                    if (obj != null)
                    {
                        GPGLItem item = obj as GPGLItem;
                        item.ActAmount += glItem.ActAmount;
                        gpItemList.Update(item);
                    }
                    else
                    {
                        gpItemList.Add(glItem);
                    }
                    
                }

                // Update the debit and Credit
                for (int i = 0; i < gpItemList.Count; i++)
                {
                    GPGLItem item = gpItemList[i] as GPGLItem;
                    if (item.ActAmount > 0)
                    {
                        item.CreditAmount = item.ActAmount;
                    }
                    else
                    {
                        item.DebitAmount = -item.ActAmount;
                    }

                    gpItemList.Update(item);

                    if (item.DebitAmount == 0 && item.CreditAmount == 0)
                        gpItemList.Remove(item);      
                }

                gpGLDocument.GLItems = gpItemList;

                GAIAM_SaveGLHistory(gpGLDocument);

                CreateGLDocument(gpGLDocument);

            }
            catch (Exception ex)
            {
                log.Error("GAIAM_ReverseGLEntriesForDeleted", ex);
            }
            finally
            {
                connection.Close();
            }            
        }
    }

    private static void GAIAM_SaveGLHistory(GPGLDocument pGlDocument)
    {
        log.Debug("GAIAM_SaveGLHistory Starting...");

        using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString))
        {
            try
            {
                // Save First
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                for (int i = 0; i < pGlDocument.GLItems.Count; i++)
                {
                    GPGLItem glItem = pGlDocument.GLItems[i] as GPGLItem;

                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = "INSERT INTO [AS_GAIAM_Chargeback_GLEntries] (AuthorizationNumber, BACHNUMB, JRNENTRY, SQNCLINE, ACTINDX, CRDTAMNT, DEBITAMT) VALUES " +
                        "(@AuthorizationNumber, @BACHNUMB, @JRNENTRY, @SQNCLINE, @ACTINDX, @CRDTAMNT, @DEBITAMT)";

                    command.Parameters.AddWithValue("@AuthorizationNumber", pGlDocument.Reference);
                    command.Parameters.AddWithValue("@BACHNUMB", pGlDocument.BatchNumber);
                    command.Parameters.AddWithValue("@JRNENTRY", pGlDocument.JournalEntry);
                    command.Parameters.AddWithValue("@SQNCLINE", ((i + 1) * 16384));
                    command.Parameters.AddWithValue("@ACTINDX", glItem.ActIndx);
                    command.Parameters.AddWithValue("@CRDTAMNT", glItem.CreditAmount);
                    command.Parameters.AddWithValue("@DEBITAMT", glItem.DebitAmount);

                    command.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                log.Error("GAIAM_SaveGLHistory", ex);
            }
            finally
            {
                connection.Close();
            }
        }

    }

    private static string GAIAM_GenerateGLBatch()
    {
        string rtn = string.Empty;

        // Try to retrieve first. If it doesn't exist, generate a new one.

        using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString))
        {
            try
            {
                // Save First
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT TOP 1 BACHNUMB FROM GL10000 WHERE BACHNUMB LIKE 'TSACCR%'";

                object obj = command.ExecuteScalar();
                if (obj == null)
                {
                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AWFConnectionString"].ConnectionString))
                    {
                        try
                        {
                            if (conn.State != ConnectionState.Open)
                                conn.Open();

                            command = conn.CreateCommand();
                            command.CommandText = "AS_WF_spGetNextInSequence";
                            command.CommandType = CommandType.StoredProcedure;


                            SqlParameter paramOutput = new SqlParameter("@Out_FormattedNumber", SqlDbType.NVarChar, 30);
                            paramOutput.Direction = ParameterDirection.Output;
                            SqlParameter paramInput = new SqlParameter("@SequenceKey", SqlDbType.VarChar, 50);
                            paramInput.Direction = ParameterDirection.Input;
                            paramInput.Value = "GAIAM GL Batch";
                            command.Parameters.Add(paramInput);
                            command.Parameters.Add(paramOutput);

                            command.ExecuteNonQuery();

                            rtn = command.Parameters["@Out_FormattedNumber"].Value.ToString();

                            log.Debug("GAIAM_GenerateCreditMemoBatch Generated Batch:" + rtn);
                        }
                        catch (Exception ex)
                        {
                            log.Error("GAIAM_GenerateCreditMemoBatch", ex);
                        }
                        finally
                        {
                            if (conn.State == ConnectionState.Open)
                                conn.Close();
                        }
                    }
                }
                else
                {
                    rtn = obj.ToString();
                }
            }
            catch (Exception ex)
            {
                log.Error("RetrieveUserDetails", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return rtn;
        }
    }

    #endregion
}