using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Dynamics.GP.eConnect;
using Microsoft.Dynamics.GP.eConnect.Serialization;

/// <summary>
/// Summary description for Helper
/// </summary>
public static class Helper
{
    private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public static bool IsDocumentForRevision(Dictionary<string, string> pDic)
    {
        bool rtn = false;

        using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AWFConnectionString"].ConnectionString))
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
            finally
            {
                connection.Close();
            }

            return rtn;
        }
    }

    public static List<string> RetrieveShippingMethods()
    {
        var rtn = new List<string>();

        //     using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString))
        using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GPConnectionString"].ConnectionString))
        {
            try
            {
                // Save First
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                SqlCommand command = connection.CreateCommand();

                const string commandText = "SELECT ShippingMethodID FROM [AS_Alere_ShippingMethods] ";

                command.CommandText = commandText;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    rtn.Add(reader[0].ToString());
                }
            }
            finally
            {
                connection.Close();
            }

            return rtn;
        }
    }

    public static string GetNextGpNumber()
    {
        using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GPConnectionString"].ConnectionString))
        {
            string rtn;
            try
            {
                // Save First
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                SqlCommand command = connection.CreateCommand();

                const string commandText = "sopGetIDNumber";
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = commandText;

                command.Parameters.AddWithValue("@I_tSOPTYPE", 2); // ORDER
                command.Parameters.AddWithValue("@I_cDocumentID", ConfigurationManager.AppSettings["OrderDocumentID"]); // From Web.Config
                command.Parameters.AddWithValue("@I_tInc_Dec", 1);

                var outputIdParam = new SqlParameter("@O_vSOPNumber", SqlDbType.VarChar, 21)
                {
                    Direction = ParameterDirection.Output
                };

                command.Parameters.Add(outputIdParam);

                command.ExecuteNonQuery();

                rtn = outputIdParam.Value.ToString().Trim();
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

        var wfDataContext = new WorkflowDataContext();

        wfDataContext.AS_WF_spGetNextInSequence(pDocumentType, ref rtn);

        return rtn;
    }

    public static List<string> UserGroup(string pUserID)
    {
        var rtn = new List<string>();

        using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AWFConnectionString"].ConnectionString))
        {
            try
            {
                // Save First
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                SqlCommand command = connection.CreateCommand();

                const string commandText = "SELECT [GroupID] FROM [AS_WF_UserGroups] WHERE [UserID] = @pUserID ";

                command.CommandText = commandText;
                command.Parameters.AddWithValue("@pUserID", pUserID);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    rtn.Add(reader[0].ToString());
                }
            }
            finally
            {
                connection.Close();
            }

            return rtn;
        }
    }

    public static GPItem RetrieveGpItemObj(string pItemSku, SqlConnection pConn)
    {
        var rtn = new GPItem();
        const string sql = "SELECT ITEMDESC, ITMSHNAM FROM [IV00101] WHERE ([ITEMNMBR] = @pItemSKU)";

        var cmd = new SqlCommand(sql, pConn);

        cmd.Parameters.AddWithValue("@pItemSKU", pItemSku);

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
            Log.Error(ex);
        }

        return rtn;
    }

    public static string RetrieveItemDescription(string pItemSku)
    {
        string rtn = string.Empty;
        const string sql = "SELECT ITEMDESC FROM [IV00101] WHERE ([ITEMNMBR] = @pItemSKU)";

        var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString);
        var cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@pItemSKU", pItemSku);

        try
        {
            conn.Open();

            object obj = cmd.ExecuteScalar();
            if (obj != null)
            {
                rtn = obj.ToString();
            }
        }
        finally
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }

        return rtn;
    }

    public static string RetrieveItemShortName(string pItemSku)
    {
        string rtn = string.Empty;
        const string sql = "SELECT ITMSHNAM FROM [IV00101] WHERE ([ITEMNMBR] = @pItemSKU)";

        var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString);
        var cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@pItemSKU", pItemSku);

        try
        {
            conn.Open();

            object obj = cmd.ExecuteScalar();
            if (obj != null)
            {
                rtn = obj.ToString();
            }
        }
        finally
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }

        return rtn;
    }

    public static string RetrieveExtenderStudioField(string pItemSku, SqlConnection pConn)
    {
        string rtn = string.Empty;
        const string sql = "SELECT eic.[Studio / Licensor] FROM dbo.extender_item_classification eic WHERE eic.[Item Number] = @pItemSKU";

        var cmd = new SqlCommand(sql, pConn);

        cmd.Parameters.AddWithValue("@pItemSKU", pItemSku);

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
            Log.Error(ex);
        }

        return rtn;
    }

    public static string RetrieveItemSkuFromShortName(string pItemShortName, SqlConnection pConn)
    {
        string rtn = string.Empty;
        const string sql = "SELECT ITEMNMBR FROM [IV00101] WHERE ([ITMSHNAM] = @pItemShortName)";

        var cmd = new SqlCommand(sql, pConn);

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
            Log.Error(ex);
        }

        return rtn;
    }

    public static List<string> RetrieveCustomerIDs()
    {
        var rtn = new List<string>();

        using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString))
        {
            try
            {
                // Save First
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                SqlCommand command = connection.CreateCommand();

                const string commandText = "SELECT TOP 500 CUSTNMBR FROM RM00101 WHERE CUSTCLAS NOT LIKE 'SPRI%' AND CUSTCLAS <> 'MUNIC/MLTY' AND CUSTCLAS <> 'TRADESHOWS' ORDER BY CUSTNMBR";

                command.CommandText = commandText;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    rtn.Add(reader[0].ToString().Trim());
                }
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
        var rtn = new List<string>();

        using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString))
        {
            try
            {
                // Save First
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                SqlCommand command = connection.CreateCommand();

                const string commandText = "SELECT TOP 500 CUSTNAME FROM RM00101 WHERE CUSTCLAS NOT LIKE 'SPRI%' AND CUSTCLAS <> 'MUNIC/MLTY' AND CUSTCLAS <> 'TRADESHOWS' ORDER BY CUSTNMBR";

                command.CommandText = commandText;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    rtn.Add(reader[0].ToString().Trim());
                }
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
        var rtn = new GPCustomer();
        const string sql = "SELECT CUSTNAME, CUSTCLAS, ADDRESS1, ADDRESS2, ADDRESS3, CITY, COUNTRY, STATE, ZIP FROM RM00101 WHERE CUSTNMBR = @pCustomerNumber";

        var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString);
        var cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@pCustomerNumber", pCustomerNumber);

        try
        {
            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                rtn = new GPCustomer
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
        finally
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }

        return rtn;
    }

    public static bool GaiamInsertDetailTemp(string pAuthorizationNumber, string pUpcCode, string pGaiamItemNumber, int pQuantity, decimal pAmountPerUnit, decimal pTotalAmount, SqlConnection pConn)
    {
        bool rtn = false;
        const string sql = "[AS_WF_spInsertIntoGaiamChargebackDetailTemp]";

        var cmd = new SqlCommand(sql, pConn) { CommandType = CommandType.StoredProcedure };

        cmd.Parameters.AddWithValue("@pAuthorizationNumber", pAuthorizationNumber);
        cmd.Parameters.AddWithValue("@pUPCCode", pUpcCode);
        cmd.Parameters.AddWithValue("@pGAIAMItemNumber", pGaiamItemNumber);
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
            Log.Error(ex);
        }
        return rtn;
    }

    public static bool GaiamProcessDetailTemp(string pAuthorizationNumber, SqlConnection pConn)
    {
        bool rtn = false;
        const string sql = "[AS_WF_spProcessGaiamChargebackDetailTemp]";

        var cmd = new SqlCommand(sql, pConn) { CommandType = CommandType.StoredProcedure, CommandTimeout = 500 };

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
            Log.Error(ex);
        }
        return rtn;
    }

    public static bool GaiamDeleteDetailTemp(string pAuthorizationNumber, SqlConnection pConn)
    {
        bool rtn = false;
        const string sql = "DELETE FROM AS_GAIAM_Chargeback_Detail_Temp WHERE [AuthorizationNumber] = @pAuthorizationNumber";

        var cmd = new SqlCommand(sql, pConn) { CommandType = CommandType.Text };

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
            Log.Error(ex);
        }
        return rtn;
    }

    public static string RetrieveVendorName(string pVendorID)
    {
        string rtn = string.Empty;
        const string sql = "SELECT VENDNAME FROM [PM00200] WHERE ([VENDORID] = @pVendorID)";

        var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString);
        var cmd = new SqlCommand(sql, conn);

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
        const string sql = "SELECT CUSTNMBR FROM [RM00101] WHERE ([CUSTNAME] = @pCustomerName)";

        var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString);
        var cmd = new SqlCommand(sql, conn);

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

        finally
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }

        return rtn;
    }

    public static bool IsItemSkuValid(string pItemSku, SqlConnection pConn)
    {
        bool rtn = false;
        const string sql = "SELECT ITEMNMBR FROM [IV00101] WHERE ([ITEMNMBR] = @pItemSKU)";

        var cmd = new SqlCommand(sql, pConn);

        cmd.Parameters.AddWithValue("@pItemSKU", pItemSku);

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
            Log.Error(ex);
        }

        return rtn;
    }

    public static bool IsItemShortNameValid(string pItemSku, string pItemShortName, SqlConnection pConn)
    {
        bool rtn = false;
        const string sql = "SELECT ITEMNMBR FROM [IV00101] WHERE ([ITEMNMBR] = @pItemSKU) AND ([ITMSHNAM] = @pItemShortName)";

        var cmd = new SqlCommand(sql, pConn);

        cmd.Parameters.AddWithValue("@pItemSKU", pItemSku);
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
            Log.Error(ex);
        }

        return rtn;
    }

    public static bool IsCustomerNumberValid(string pCustomerNumber)
    {
        bool rtn = false;
        const string sql = "SELECT CUSTNMBR FROM [RM00101] WHERE ([CUSTNMBR] = @pCustomerNumber)";

        var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString);
        var cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@pCustomerNumber", pCustomerNumber);

        try
        {
            conn.Open();

            object obj = cmd.ExecuteScalar();
            if (obj != null)
                rtn = true;
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
        const string sql = "SELECT SLPRSNID FROM RM00301 WHERE ([SLPRSNID] = @pSLPRSNID)";

        var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString);
        var cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@pSLPRSNID", pSalesPersonID);

        try
        {
            conn.Open();

            object obj = cmd.ExecuteScalar();
            if (obj != null)
                rtn = true;
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
        var rtn = new WorkflowUser();
        const string sql = "SELECT [UserPassword], [ERPUserID], [FirstName], [LastName], [Department], [Company], " +
                           "[Phone], [EmailAdd], [UserRole], [CompanyPosition] FROM [AWF].[dbo].[AS_WF_Users] WHERE ([UserID] = @pUserID)";

        var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AWFConnectionString"].ConnectionString);
        var cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@pUserID", pUserID);

        try
        {
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                rtn = new WorkflowUser
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
        finally
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }

        return rtn;
    }

    public static string RetrieveWfCommentFromDocKeyString(string pDocKeyStr1)
    {
        string rtn = string.Empty;

        const string sql = "SELECT [Comment] FROM [AWF].[dbo].[AS_WF_WorkflowInstance] WHERE ([DocKey_Str1] = @pDocKey_Str1)";

        var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AWFConnectionString"].ConnectionString);
        var cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@pDocKey_Str1", pDocKeyStr1);

        try
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            object obj = cmd.ExecuteScalar();

            if (obj != null)
                rtn = obj.ToString();
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

        const string sql = "SELECT COUNT(*) FROM AS_WF_UserGroups LEFT OUTER JOIN AS_WF_GroupsWorkflowDocumentTypes ON AS_WF_UserGroups.GroupID = AS_WF_GroupsWorkflowDocumentTypes.GroupID " +
                           "WHERE (AS_WF_UserGroups.UserID = @pUserID and AS_WF_GroupsWorkflowDocumentTypes.DocType = @pDocType)";

        var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AWFConnectionString"].ConnectionString);
        var cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@pUserID", pUserID);
        cmd.Parameters.AddWithValue("@pDocType", pDocType);

        try
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            object obj = cmd.ExecuteScalar();
            if (Convert.ToInt32(obj) > 0)
                rtn = true;
        }
        finally
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }

        return rtn;
    }

    #region Econnect

    private static string SerializeGlDocumentObject(StringWriter pStringWriter, GPGLDocument pGlDocument)
    {
        Log.Debug("SerializeGLDocumentObject: Starting");

        try
        {
            GPGLItemList glItems = pGlDocument.GLItems;
            var eConnect = new Microsoft.Dynamics.GP.eConnect.Serialization.eConnectType();
            var glHeader = new taGLTransactionHeaderInsert();
            var itemsArray = new taGLTransactionLineInsert_ItemsTaGLTransactionLineInsert[glItems.Count];

            var gltype = new GLTransactionType();

            var serializer = new XmlSerializer(eConnect.GetType());

            glHeader.BACHNUMB = pGlDocument.BatchNumber;
            glHeader.JRNENTRY = pGlDocument.JournalEntry == 0 ? Convert.ToInt32(GenerateNextGpNumber("GL")) : pGlDocument.JournalEntry;
            glHeader.REFRENCE = pGlDocument.Reference;
            glHeader.TRXDATE = pGlDocument.TrxDate == DateTime.MinValue ? DateTime.Now.ToShortDateString() : pGlDocument.TrxDate.ToShortDateString();
            glHeader.TRXTYPE = Convert.ToInt16(pGlDocument.TrxType);
            glHeader.SERIES = Convert.ToInt16(pGlDocument.Series);

            Log.Debug("SerializeGLDocumentObject: glItems Count = " + glItems.Count);

            for (int i = 0; i < glItems.Count; i++)
            {
                var glLine = new taGLTransactionLineInsert_ItemsTaGLTransactionLineInsert();
                var glItem = (GPGLItem)glItems[i];
                glLine.BACHNUMB = pGlDocument.BatchNumber;
                glLine.JRNENTRY = glHeader.JRNENTRY;
                glLine.SQNCLINE = glItem.SqncLine == 0 ? ((i + 1) * 16384) : glItem.SqncLine;
                glLine.ACTINDX = glItem.ActIndx;
                glLine.CRDTAMNT = glItem.CreditAmount;
                glLine.DEBITAMT = glItem.DebitAmount;
                glLine.ACTNUMST = glItem.ActNumSt;
                glLine.DSCRIPTN = glItem.Description;
                glLine.DOCDATE = glHeader.TRXDATE;

                itemsArray[i] = glLine;

                Log.Debug("SerializeGLDocumentObject: GLItem:" + glLine.SQNCLINE + ":" + glLine.ACTINDX + ":" + glLine.CRDTAMNT + ":" + glLine.DEBITAMT);
            }

            gltype.taGLTransactionHeaderInsert = glHeader;
            gltype.taGLTransactionLineInsert_Items = itemsArray;

            GLTransactionType[] myGlMaster = { gltype };
            eConnect.GLTransactionType = myGlMaster;

            serializer.Serialize(pStringWriter, eConnect);
        }
        catch (eConnectException ex2)
        {
            Log.Error("SerializeGLDocumentObject: eConnectException - ", ex2);
            throw;
        }
        catch (SqlException ex1)
        {
            Log.Error("SerializeGLDocumentObject: SqlException - ", ex1);
            throw;
        }
        catch (Exception ex)
        {
            Log.Error("SerializeGLDocumentObject: Exception - ", ex);
            throw;
        }

        return pStringWriter.ToString();
    }

    public static bool CreateGlDocument(GPGLDocument pGlDocument)
    {
        bool rtn = false;

        try
        {
            var stringWriter = new StringWriter();

            string inputDocument = SerializeGlDocumentObject(stringWriter, pGlDocument);

            if (inputDocument.Trim().Length > 0)
                rtn = EntryPoint(inputDocument);
        }
        catch (Exception ex)
        {
            Log.Error("CreateSOPDocument", ex);
        }

        return rtn;
    }

    private static bool EntryPoint(string pInputDocument)
    {
        bool rtn = false;
        var e = new eConnectMethods();
        string sXsdSchema = string.Empty;

        Log.Debug("EntryPoint: pInputDocument =" + pInputDocument);

        try
        {
            var xDoc = new XmlDocument();
            xDoc.Load(@"C:\Program Files\Common Files\microsoft shared\eConnect 10\XML Sample Documents\Incoming XSD Schemas\eConnect.xsd");
            sXsdSchema = xDoc.OuterXml;
        }
        catch (Exception ex)
        {
            Log.Error("EntryPoint", ex);
        }

        var builder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["EconnectConnString"].ConnectionString);
        string ds = builder.DataSource;
        string catalog = builder.InitialCatalog;

        string strConnection = "data source=" + ds + ";initial catalog=" + catalog + ";integrated security=SSPI;persist security info=False;packet size=4096;";

        //strConnection = CompanyConnectionString;

        try
        {
            rtn = e.eConnect_EntryPoint(strConnection, EnumTypes.ConnectionStringType.SqlClient, pInputDocument, EnumTypes.SchemaValidationType.XSD, sXsdSchema);
        }
        catch (eConnectException ex1)
        {
            Log.Error("EntryPoint: eConnectException", ex1);
        }
        catch (SqlException ex2)
        {
            Log.Error("EntryPoint: SqlException", ex2);
        }
        catch (Exception ex)
        {
            Log.Error("EntryPoint: Exception", ex);
        }
        finally
        {
            e.Dispose();
        }
        return rtn;
    }

    private static string GenerateNextGpNumber(string pDocumentType)
    {
        string rtn = string.Empty;

        var builder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["EconnectConnString"].ConnectionString);
        string ds = builder.DataSource;
        string catalog = builder.InitialCatalog;

        string strConnection = "data source=" + ds + ";initial catalog=" + catalog + ";integrated security=SSPI;persist security info=False;packet size=4096;";

        var nextDocNumbers = new Microsoft.Dynamics.GP.eConnect.MiscRoutines.GetNextDocNumbers();

        switch (pDocumentType)
        {
            case "GL":
                rtn = nextDocNumbers.GetNextGLJournalEntryNumber(Microsoft.Dynamics.GP.eConnect.MiscRoutines.GetNextDocNumbers.IncrementDecrement.Increment, strConnection);
                break;
        }

        return rtn;
    }

    public static void GaiamReverseGlEntriesForDeleted(string pAuthorizationNumber)
    {
        var gpGlDocument = new GPGLDocument();
        var gpItemList = new GPGLItemList();

        using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString))
        {
            try
            {
                // Save First
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT JRNENTRY, ACTINDX, CRDTAMNT, DEBITAMT FROM [AS_GAIAM_Chargeback_GLEntries] WHERE AuthorizationNumber = @pReference ORDER BY SQNCLINE";
                command.Parameters.AddWithValue("@pReference", pAuthorizationNumber);
                var da = new SqlDataAdapter();
                var ds = new DataSet();
                da.SelectCommand = command;

                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    gpGlDocument.Reference = pAuthorizationNumber;
                    gpGlDocument.BatchNumber = GaiamGenerateGlBatch();

                    var glItem = new GPGLItem { ActIndx = Convert.ToInt32(ds.Tables[0].Rows[i]["ACTINDX"].ToString()) };

                    if (Convert.ToDecimal(ds.Tables[0].Rows[i]["CRDTAMNT"].ToString()) > 0)

                        glItem.ActAmount -= Convert.ToDecimal(ds.Tables[0].Rows[i]["CRDTAMNT"].ToString());
                    else
                        glItem.ActAmount += Convert.ToDecimal(ds.Tables[0].Rows[i]["DEBITAMT"].ToString());

                    object obj = gpItemList[gpItemList.IndexOfKey(Convert.ToInt32(ds.Tables[0].Rows[i]["ACTINDX"].ToString()))];

                    // Existing record
                    if (obj != null)
                    {
                        var item = obj as GPGLItem;
                        if (item != null)
                        {
                            item.ActAmount += glItem.ActAmount;
                            gpItemList.Update(item);
                        }
                    }
                    else
                    {
                        gpItemList.Add(glItem);
                    }
                }

                // Update the debit and Credit
                for (int i = 0; i < gpItemList.Count; i++)
                {
                    var item = gpItemList[i] as GPGLItem;
                    if (item != null)
                    {
                        if (item.ActAmount > 0)

                            item.CreditAmount = item.ActAmount;

                        else

                            item.DebitAmount = -item.ActAmount;

                        gpItemList.Update(item);

                        if ((item.DebitAmount == 0 && item.CreditAmount == 0))
                            gpItemList.Remove(item);
                    }
                }

                gpGlDocument.GLItems = gpItemList;

                GaiamSaveGlHistory(gpGlDocument);

                CreateGlDocument(gpGlDocument);
            }
            catch (Exception ex)
            {
                Log.Error("GAIAM_ReverseGLEntriesForDeleted", ex);
            }
            finally
            {
                connection.Close();
            }
        }
    }

    private static void GaiamSaveGlHistory(GPGLDocument pGlDocument)
    {
        Log.Debug("GAIAM_SaveGLHistory Starting...");

        using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString))
        {
            try
            {
                // Save First
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                for (int i = 0; i < pGlDocument.GLItems.Count; i++)
                {
                    var glItem = pGlDocument.GLItems[i] as GPGLItem;
                    if (glItem != null)
                    {
                        SqlCommand command = connection.CreateCommand();
                        command.CommandText =
                            "INSERT INTO [AS_GAIAM_Chargeback_GLEntries] (AuthorizationNumber, BACHNUMB, JRNENTRY, SQNCLINE, ACTINDX, CRDTAMNT, DEBITAMT) VALUES " +
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
            }
            catch (Exception ex)
            {
                Log.Error("GAIAM_SaveGLHistory", ex);
            }
            finally
            {
                connection.Close();
            }
        }
    }

    private static string GaiamGenerateGlBatch()
    {
        string rtn = string.Empty;

        // Try to retrieve first. If it doesn't exist, generate a new one.

        using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString))
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
                    using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AWFConnectionString"].ConnectionString))
                    {
                        try
                        {
                            if (conn.State != ConnectionState.Open)
                                conn.Open();

                            command = conn.CreateCommand();
                            command.CommandText = "AS_WF_spGetNextInSequence";
                            command.CommandType = CommandType.StoredProcedure;

                            var paramOutput = new SqlParameter("@Out_FormattedNumber", SqlDbType.NVarChar, 30) { Direction = ParameterDirection.Output };
                            var paramInput = new SqlParameter("@SequenceKey", SqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = "GAIAM GL Batch" };
                            command.Parameters.Add(paramInput);
                            command.Parameters.Add(paramOutput);

                            command.ExecuteNonQuery();

                            rtn = command.Parameters["@Out_FormattedNumber"].Value.ToString();

                            Log.Debug("GAIAM_GenerateCreditMemoBatch Generated Batch:" + rtn);
                        }
                        catch (Exception ex)
                        {
                            Log.Error("GAIAM_GenerateCreditMemoBatch", ex);
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
                Log.Error("RetrieveUserDetails", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return rtn;
        }
    }

    #endregion Econnect
}