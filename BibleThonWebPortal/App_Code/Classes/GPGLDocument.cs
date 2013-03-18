using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

/// <summary>
/// Summary description for GPCustomer
/// </summary>
public class GPGLDocument
{
    #region Private Variables

    //first page                          
    private string batchNumber = string.Empty;
    private int journalEntry = 0;
    private string reference = string.Empty;
    private DateTime trxDate = DateTime.MinValue;
    private DateTime rvrsngDate = Convert.ToDateTime(System.DateTime.Today.ToShortDateString());
    private int trxType = 0;
    private decimal sqncLine = 0;
    private int series = 1;

    private GPGLItemList glItems = new GPGLItemList();

    #endregion

    #region Public properties

    //first page
    [XmlElement()]
    public string BatchNumber { get { return batchNumber; } set { batchNumber = value; } }
    public int JournalEntry { get { return journalEntry; } set { journalEntry = value; } }
    public string Reference { get { return reference; } set { reference = value; } }
    public DateTime TrxDate { get { return trxDate; } set { trxDate = value; } }
    public DateTime RvrsngDate { get { return rvrsngDate; } set { rvrsngDate = value; } }
    public int TrxType { get { return trxType; } set { trxType = value; } }
    public decimal SqncLine { get { return sqncLine; } set { sqncLine = value; } }
    public int Series { get { return series; } set { series = value; } }

    public GPGLItemList GLItems { get { return glItems; } set { glItems = value; } }

    #endregion

    #region Constructor
    public GPGLDocument()
    {

    }
    #endregion
}