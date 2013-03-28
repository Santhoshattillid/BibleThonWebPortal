namespace AlbaDL
{
    /// <summary>
    /// Definition of Order Items
    /// </summary>
    public class OrderItems
    {
        public short SOPTYPE { get; set; }

        public string SOPNUMBE { get; set; }

        public string CUSTNMBR { get; set; }

        public string DOCDATE { get; set; }

        public string ITEMNMBR { get; set; }

        public decimal UNITPRCE { get; set; }

        public decimal XTNDPRCE { get; set; }

        public decimal QUANTITY { get; set; }

        public decimal UNITCOST { get; set; }

        public string ITEMDESC { get; set; }

        public short NONINVEN { get; set; }

        public string SLPRSNID { get; set; }

        public decimal TOTALQTY { get; set; }

        public string CURNCYID { get; set; }

        public string UOFM { get; set; }

        public string ShipToName { get; set; }

        public string ADDRESS1 { get; set; }

        public string ADDRESS2 { get; set; }

        public string ADDRESS3 { get; set; }

        public string CITY { get; set; }

        public string STATE { get; set; }

        public string ZIPCODE { get; set; }

        public string COUNTRY { get; set; }

        public string PHNUMBR1 { get; set; }

        public short SOPSTATUS { get; set; }
    }
}