using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Xml;
using System.Xml.Xsl;
using System.IO;

public partial class DocumentViewer : System.Web.UI.Page
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);    

    protected void Page_Load(object sender, EventArgs e)
    {
        log.Debug("Starting Document Viewer");

        string wfInstanceID = string.Empty;
        string documentType = string.Empty;
        string headerQuery = string.Empty;
        string[] detailQuery = new string[5];
        
        string[] docKeyStr = new string[5];
        int[] docKeyInt = new int[5];
        decimal[] docKeyDec = new decimal[5];

        string sqlQuery = string.Empty;

        if (!string.IsNullOrEmpty(Request.QueryString["WFInstanceID"]))
            wfInstanceID = Request.QueryString["WFInstanceID"];

        if (!string.IsNullOrEmpty(Request.QueryString["DocumentType"]))
            documentType = Request.QueryString["DocumentType"];


        SqlConnection dbCompanyConn = OpenCompanyDBConnection();
            SqlConnection dbWFConn = OpenWFDBConnection();

        try
        {                        
            //Retrieve the SQL Queries from the WORKFLOW Setup table
            RetrieveWFQuery(wfInstanceID, out headerQuery, out detailQuery);

            dbCompanyConn.Open();            

            //sqlQuery = headerQuery + " WHERE " + RetrieveKey1Name(documentType) + " = '" + documentKey + "'";


            //Added to make Queries more generic
            RetrieveKeys(wfInstanceID, out docKeyStr, out docKeyInt, out docKeyDec);            
            
            for (int i = 0; i < 5; i++)
            {
                string docKeyString = String.Format("@DocKey_Str{0}", (i + 1).ToString());
                string docIntString = String.Format("@DocKey_Int{0}", (i + 1).ToString());
                string docDecString = String.Format("@DocKey_Dec{0}", (i + 1).ToString());

                headerQuery = headerQuery.Replace(docKeyString, docKeyStr[i]);
                headerQuery = headerQuery.Replace(docIntString, docKeyInt[i].ToString());
                headerQuery = headerQuery.Replace(docDecString, docKeyDec[i].ToString());    
            }

            sqlQuery = headerQuery;

            SqlCommand sqlCmd = new SqlCommand(sqlQuery, dbCompanyConn);
            SqlDataReader headerDR = sqlCmd.ExecuteReader();

            string docXML = @"<?xml version=""1.0"" encoding=""utf-8"" ?>" + System.Environment.NewLine + "<Document>";

            DataTable headerDT = new DataTable("Header");
            headerDT.Load(headerDR);

            System.IO.StringWriter xmlWriter = new System.IO.StringWriter();
            headerDT.WriteXml(xmlWriter);

            headerDR.Close();

            string headerXML = xmlWriter.ToString();
            headerXML = headerXML.Replace("<DocumentElement>", "");
            headerXML = headerXML.Replace("</DocumentElement>", "");

            docXML += headerXML;

            dbCompanyConn.Close();

            for (int i = 0; i < 5; i++)
            {                                
                for (int x = 0; x < 5; x++)
                {
                    string docKeyString = String.Format("@DocKey_Str{0}", (x + 1).ToString());
                    string docIntString = String.Format("@DocKey_Int{0}", (x + 1).ToString());
                    string docDecString = String.Format("@DocKey_Dec{0}", (x + 1).ToString());

                    detailQuery[i] = detailQuery[i].Replace(docKeyString, docKeyStr[x]);
                    detailQuery[i] = detailQuery[i].Replace(docIntString, docKeyInt[x].ToString());
                    detailQuery[i] = detailQuery[i].Replace(docDecString, docKeyDec[x].ToString());    
                }

                sqlQuery = detailQuery[i];

                if (!string.IsNullOrEmpty(sqlQuery))
                {
                    sqlCmd = new SqlCommand(sqlQuery, dbCompanyConn);

                    dbCompanyConn.Open();

                    SqlDataReader detailDR = sqlCmd.ExecuteReader();

                    DataTable detailDT = new DataTable("LineItem");
                    detailDT.Load(detailDR);

                    xmlWriter = new System.IO.StringWriter();
                    detailDT.WriteXml(xmlWriter);

                    detailDR.Close();

                    string detailXML = xmlWriter.ToString();
                    detailXML = detailXML.Replace("<DocumentElement>", "<Details" + (i + 1).ToString() + ">");
                    detailXML = detailXML.Replace("</DocumentElement>", "</Details" + (i + 1).ToString() + ">");

                    docXML += detailXML;

                    dbCompanyConn.Close();
                }
            }

            #region OLD CODE
            //// Detail1
            //if (!string.IsNullOrEmpty(detailQuery1))
            //{
            //    sqlQuery = detailQuery1 + " WHERE " + RetrieveKey1Name(documentType) + " = '" + documentKey + "'";
            //    sqlCmd = new SqlCommand(sqlQuery, dbCompanyConn);

            //    dbCompanyConn.Open();

            //    SqlDataReader detailDR = sqlCmd.ExecuteReader();

            //    DataTable detailDT = new DataTable("LineItem");
            //    detailDT.Load(detailDR);

            //    xmlWriter = new System.IO.StringWriter();
            //    detailDT.WriteXml(xmlWriter);

            //    detailDR.Close();

            //    string detailXML = xmlWriter.ToString();
            //    detailXML = detailXML.Replace("<DocumentElement>", "<Details1>");
            //    detailXML = detailXML.Replace("</DocumentElement>", "</Details1>");

            //    docXML += detailXML;

            //    dbCompanyConn.Close();   
            //}

            ////Detail2
            //if (!string.IsNullOrEmpty(detailQuery2))
            //{                
            //    sqlQuery = detailQuery2 + " WHERE " + RetrieveKey1Name(documentType) + " = '" + documentKey + "'";
            //    sqlCmd = new SqlCommand(sqlQuery, dbCompanyConn);
            //    dbCompanyConn.Open();
            //    SqlDataReader detailDR = sqlCmd.ExecuteReader();

            //    DataTable detailDT = new DataTable("LineItem");
            //    detailDT.Load(detailDR);

            //    xmlWriter = new System.IO.StringWriter();
            //    detailDT.WriteXml(xmlWriter);

            //    detailDR.Close();

            //    string detailXML = xmlWriter.ToString();
            //    detailXML = detailXML.Replace("<DocumentElement>", "<Details2>");
            //    detailXML = detailXML.Replace("</DocumentElement>", "</Details2>");

            //    docXML += detailXML;
            //}

            ////Detail3
            //if (!string.IsNullOrEmpty(detailQuery3))
            //{
            //    sqlQuery = detailQuery3 + " WHERE " + RetrieveKey1Name(documentType) + " = '" + documentKey + "'";
            //    sqlCmd = new SqlCommand(sqlQuery, dbCompanyConn);
            //    dbCompanyConn.Open();
            //    SqlDataReader detailDR = sqlCmd.ExecuteReader();

            //    DataTable detailDT = new DataTable("LineItem");
            //    detailDT.Load(detailDR);

            //    xmlWriter = new System.IO.StringWriter();
            //    detailDT.WriteXml(xmlWriter);

            //    detailDR.Close();

            //    string detailXML = xmlWriter.ToString();
            //    detailXML = detailXML.Replace("<DocumentElement>", "<Details3>");
            //    detailXML = detailXML.Replace("</DocumentElement>", "</Details3>");

            //    docXML += detailXML;

            //    dbCompanyConn.Close();   
            //}

            ////Detail4
            //if (!string.IsNullOrEmpty(detailQuery4))
            //{
            //    sqlQuery = detailQuery4 + " WHERE " + RetrieveKey1Name(documentType) + " = '" + documentKey + "'";
            //    sqlCmd = new SqlCommand(sqlQuery, dbCompanyConn);
            //    dbCompanyConn.Open();
            //    SqlDataReader detailDR = sqlCmd.ExecuteReader();

            //    DataTable detailDT = new DataTable("LineItem");
            //    detailDT.Load(detailDR);

            //    xmlWriter = new System.IO.StringWriter();
            //    detailDT.WriteXml(xmlWriter);

            //    detailDR.Close();

            //    string detailXML = xmlWriter.ToString();
            //    detailXML = detailXML.Replace("<DocumentElement>", "<Details4>");
            //    detailXML = detailXML.Replace("</DocumentElement>", "</Details4>");

            //    docXML += detailXML;

            //    dbCompanyConn.Close();   
            //}

            ////Detail5
            //if (!string.IsNullOrEmpty(detailQuery5))
            //{
            //    sqlQuery = detailQuery5 + " WHERE " + RetrieveKey1Name(documentType) + " = '" + documentKey + "'";
            //    sqlCmd = new SqlCommand(sqlQuery, dbCompanyConn);
            //    dbCompanyConn.Open();
            //    SqlDataReader detailDR = sqlCmd.ExecuteReader();

            //    DataTable detailDT = new DataTable("LineItem");
            //    detailDT.Load(detailDR);

            //    xmlWriter = new System.IO.StringWriter();
            //    detailDT.WriteXml(xmlWriter);

            //    detailDR.Close();

            //    string detailXML = xmlWriter.ToString();
            //    detailXML = detailXML.Replace("<DocumentElement>", "<Details5>");
            //    detailXML = detailXML.Replace("</DocumentElement>", "</Details5>");

            //    docXML += detailXML;

            //    dbCompanyConn.Close();   
            //}
            #endregion

            docXML += System.Environment.NewLine + "</Document>";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(docXML);

            string pathToCreate = Server.MapPath("~/StyleSheets/" + documentType + ".xsl");

            string resultHTML = ConvertXML(xmlDoc, pathToCreate, null);     
       
            Panel1.Controls.Add(new LiteralControl(resultHTML));
            //string resultHTML = TransformXMLToHTML(docXML);
        }
        catch (Exception ex)
        {
            log.Error("Page_Load", ex);
        }
        finally
        {
            if (dbCompanyConn.State == ConnectionState.Open)
                dbCompanyConn.Close();

            if (dbWFConn.State == ConnectionState.Open)
                dbWFConn.Close();
        }

    }

    private void RetrieveKeys(string pWFInstanceID, out string[] pDocKeyStr, out int[] pDocKeyInt, out decimal[] pDocKeyDec)
    {        
        SqlConnection dbWFConn = OpenWFDBConnection();

        string[] docKeyStr = new string[5];
        int[] docKeyInt = new int[5];
        decimal[] docKeyDec = new decimal[5];


        try
        {
            //Retrieve the Workflow ID first
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT [DocKey_Str1], [DocKey_Str2], [DocKey_Str3], [DocKey_Str4], [DocKey_Str5], " +
                "[DocKey_Int1], [DocKey_Int2], [DocKey_Int3], [DocKey_Int4], [DocKey_Int5], " +
                "[DocKey_Dec1], [DocKey_Dec2], [DocKey_Dec3], [DocKey_Dec4], [DocKey_Dec5] " +
                "FROM AS_WF_WorkflowInstance WHERE WFInstanceID = @pWFInstanceID";
            cmd.Connection = dbWFConn;
            cmd.Parameters.AddWithValue("@pWFInstanceID", pWFInstanceID);

            dbWFConn.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                docKeyStr[0] = reader[0].ToString();
                docKeyStr[1] = reader[1].ToString();
                docKeyStr[2] = reader[2].ToString();
                docKeyStr[3] = reader[3].ToString();
                docKeyStr[4] = reader[4].ToString();

                docKeyInt[0] = reader[5] == DBNull.Value ? 0 : Convert.ToInt32(reader[5].ToString());
                docKeyInt[1] = reader[6] == DBNull.Value ? 0 : Convert.ToInt32(reader[5].ToString());
                docKeyInt[2] = reader[7] == DBNull.Value ? 0 : Convert.ToInt32(reader[5].ToString());
                docKeyInt[3] = reader[8] == DBNull.Value ? 0 : Convert.ToInt32(reader[5].ToString());
                docKeyInt[4] = reader[9] == DBNull.Value ? 0 : Convert.ToInt32(reader[5].ToString());

                docKeyDec[0] = reader[10] == DBNull.Value ? 0 : Convert.ToDecimal(reader[10].ToString());
                docKeyDec[1] = reader[11] == DBNull.Value ? 0 : Convert.ToDecimal(reader[11].ToString());
                docKeyDec[2] = reader[12] == DBNull.Value ? 0 : Convert.ToDecimal(reader[12].ToString());
                docKeyDec[3] = reader[13] == DBNull.Value ? 0 : Convert.ToDecimal(reader[13].ToString());
                docKeyDec[4] = reader[14] == DBNull.Value ? 0 : Convert.ToDecimal(reader[14].ToString());
            }            
        }
        catch (Exception ex)
        {
            log.Error("RetrieveKeys", ex);
        }
        finally
        {
            if (dbWFConn.State == ConnectionState.Open)
                dbWFConn.Close();
        }

        pDocKeyStr = docKeyStr;
        pDocKeyInt = docKeyInt;
        pDocKeyDec = docKeyDec;
    }

    private void RetrieveWFQuery(string pWFInstanceID, out string pHeaderQuery, out string[] pDetailQuery)
    {        
        string wfID = string.Empty;
        string headerQuery = string.Empty;
        string[] detailQuery = new string[5];        

        SqlConnection dbWFConn = OpenWFDBConnection();

        try
        {
            //Retrieve the Workflow ID first
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT WFID FROM AS_WF_WorkflowInstance WHERE WFInstanceID = @pWFInstanceID";
            cmd.Connection = dbWFConn;
            cmd.Parameters.AddWithValue("@pWFInstanceID", pWFInstanceID);

            dbWFConn.Open();

            wfID = cmd.ExecuteScalar().ToString();            

            if (wfID != null && wfID.Length > 0)
            {
                cmd = new SqlCommand();
                cmd.CommandText = "SELECT HeaderQuery, DetailQuery1, DetailQuery2, DetailQuery3, DetailQuery4, DetailQuery5 FROM AS_WF_WorkflowSetup WHERE WFID = @WFID";
                cmd.Connection = dbWFConn;
                cmd.Parameters.AddWithValue("@WFID", wfID);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    headerQuery = reader[0].ToString();

                    detailQuery[0] = reader[1].ToString();
                    detailQuery[1] = reader[2].ToString();
                    detailQuery[2] = reader[3].ToString();
                    detailQuery[3] = reader[4].ToString();
                    detailQuery[4] = reader[5].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("RetrieveKey1Name", ex);
        }
        finally
        {
            if (dbWFConn.State == ConnectionState.Open)
                dbWFConn.Close();
        }

        pHeaderQuery = headerQuery;
        pDetailQuery = detailQuery;
    }

    private SqlConnection OpenCompanyDBConnection()
    {
        string connString = ConfigurationManager.ConnectionStrings["TWOConnectionString"].ConnectionString;
        SqlConnection dbConn = new SqlConnection(connString);           

        return dbConn;
    }

    private SqlConnection OpenWFDBConnection()
    {
        string connString = ConfigurationManager.ConnectionStrings["AWFConnectionString"].ConnectionString;
        SqlConnection dbConn = new SqlConnection(connString);        

        return dbConn;
    }

    private string RetrieveKey1Name(string pDocumentType)
    {
        string rtn = string.Empty;
        SqlConnection dbWFConn = OpenWFDBConnection();

        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT STRKEYNAME1 FROM AS_WF_WorkflowDocumentTypes WHERE DOCTYPE = @pDocumentType";
            cmd.Connection = dbWFConn;
            cmd.Parameters.AddWithValue("@pDocumentType", pDocumentType);

            dbWFConn.Open();

            rtn = cmd.ExecuteScalar().ToString();
        }
        catch (Exception ex)
        {
            log.Error("RetrieveKey1Name", ex);
        }
        finally
        {
            if (dbWFConn.State == ConnectionState.Open)
                dbWFConn.Close();
        }

        return rtn;
    }

    public static string ConvertXML(XmlDocument InputXMLDocument, string XSLTFilePath, XsltArgumentList XSLTArgs)
    {
        System.IO.StringWriter sw = new System.IO.StringWriter();
        XslCompiledTransform xslTrans = new XslCompiledTransform();
        xslTrans.Load(XSLTFilePath);
        xslTrans.Transform(InputXMLDocument.CreateNavigator(), XSLTArgs, sw);
        return sw.ToString();
    }

    public static string ConvertXML(string InputXMLDocument, string XSLTFilePath, XsltArgumentList XSLTArgs)
    {
        System.IO.StringWriter sw = new System.IO.StringWriter();
        XslCompiledTransform xslTrans = new XslCompiledTransform();
        xslTrans.Load(XSLTFilePath);

        using (XmlReader reader = XmlReader.Create(new StringReader(InputXMLDocument)))
        {
            xslTrans.Transform(reader, null, sw);
        }
        return sw.ToString();
    }  


}