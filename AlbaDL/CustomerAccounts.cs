namespace AlbaDL
{
    /// <summary>
    /// Class for Customer accounts
    /// </summary>
    public class CustomerAccounts
    {
        public string CashAccount { get; set; }

        public string AccountReceivable { get; set; }

        public string SalesAccount { get; set; }

        public string CostofSalesAccount { get; set; }

        public string InventoryAccount { get; set; }

        public string TermDiscountTackenAccount { get; set; }

        public string TermDiscountAvailableAccount { get; set; }

        public string FinanceChargesAccount { get; set; }

        public string WriteOffAccount { get; set; }

        public string SalesOrderReturnAccount { get; set; }

        public string OverPaymentWriteOffAccount { get; set; }

        public short DefaultAccountType { get; set; }
    }
}