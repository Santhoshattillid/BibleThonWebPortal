using System;

namespace AlbaDL
{
    public class NewCustomer : CustomerDetails
    {
        public string CustomerClass { get; set; }

        public string CustomerPriority { get; set; }

        public string UpsZone { get; set; }

        public string ShippingMethod { get; set; }

        public string TaxScheduleId { get; set; }

        public short ShipComplete { get; set; } //Ship complete documents: 0=False; 1=True Default is zero on new record

        public string PrimaryShipToAddressCode { get; set; }

        public string PrimaryBillToAddressCode { get; set; }

        public string StatementToAddressCode { get; set; }

        public string SalesPersonId { get; set; }

        public string SalesTerritorry { get; set; }

        public string UserDefined1 { get; set; }

        public string UserDefined2 { get; set; }

        public string Comment1 { get; set; }

        public string Comment2 { get; set; }

        public decimal TradeDiscount { get; set; }

        public string PaymentTermId { get; set; }

        public short DiscountGracePeriod { get; set; }

        public short DuedateGracePeriod { get; set; }

        public string PriceLevel { get; set; }

        public string NoteText { get; set; }

        public short BalanceType { get; set; }

        //Balance type: 0=Open item; 1=Balance forward Default is zero on new record
        public short FinanceChargeType { get; set; }

        public int FinanceChargePercent { get; set; } //Finance charge percent; used if FNCHATYP=1

        public decimal FinanceChargeDollar { get; set; } //Finance charge dollar; used if FNCHATYP=2

        public short MinPaymentType { get; set; }

        //Minimum payment type: 0=No minimum; 1=Percent; 2=Amount Default is zero for new record
        public short MinPymntPercent { get; set; } //Minimum payment percent; used if MINPYTYP=1

        public decimal MinPymntDollarAmt { get; set; } //Minimum payment dollar amount; used if MINPYTYP=2

        public short CreditLimitType { get; set; }

        //Credit limit type:0=No credit; 1=Unlimited;2=Amount Default is zero on new record
        public decimal CreditLimitAmt { get; set; } //Credit limit amount; used if CRLMTTYP=2

        public short CreditLimitPeriod { get; set; }

        //Credit limit period; used if CRLMTTYP=2 and the credit limit warning is used in Microsoft Dynamics GP application
        public decimal CreditLimitPeriodAmat { get; set; }

        //Credit limit period amount; used if CRLMTTYP=2 and the credit limit warning is used in Microsoft Dynamics GP application
        public short MaximumWriteOffType { get; set; }

        //Maximum write-off type: 0=Not allowed; 1=Unlimited; 2=Maximum Default is zero for new record
        public decimal MaximumWriteOffAmt { get; set; } //Maximum write-off amount; used if MXWOFTYP=2

        public short RevalueCustomer { get; set; }

        //Revalue customer: 0=Do not revalue; 1=Revalue Default is one for new record
        public short PostResultTo { get; set; }

        //Post results to: 0=Receivables/Discount Account;1=Sales offset Default is zero on new records
        public short OrderFullFillmentShortage { get; set; }

        //Order fulfillment shortage: 1=None; 2=Back order remaining; 3=Cancel remaining Default is one on new record
        public short IncludeinDemandPlanning { get; set; }

        public string BankBranch { get; set; }

        public short UserLanguage { get; set; }

        public string TaxExcempt1 { get; set; }

        public string TaxExcempt2 { get; set; }

        public string TaxRegNumber { get; set; }

        public string RateTypeId { get; set; }

        public short StatementCycleId { get; set; }

        //Statement Cycle: 1=No Statement; 2=Weekly;3=Biweekly; 4=Semimonthly;5=Monthly;6=Bimonthly; 7=Quarterly
        public short MaintainHistoryCalendarYear { get; set; }

        //Maintain history--calendar year:0=Do not maintain history;1=Maintain history Default is 1 on new record
        public short MaintainHistoryFiscalYear { get; set; }

        //Maintain history--fiscal year: 0=Do not maintain history;1=Maintain history Default is 1 on new record
        public short MaintainHistoryTrans { get; set; }

        // Maintain history--transaction: 0=Do not maintain history; 1=Maintain history Default is 1 on new record
        public short MaintainHistoryDist { get; set; }

        //Maintain history--distribution: 0=Do not maintain history; 1=Maintain history Default is 1 on new record
        public short SendEmailStatment { get; set; }

        //Send e-mail statements: 0=Do not sent statements;1=Send statements Default is zero on new record
        public string ToReceipient { get; set; }

        public string CcReceiptient { get; set; }

        public string BcCReceipient { get; set; }

        public string CheckBookId { get; set; }

        public short DefaultCashAccount { get; set; }

        //Cash account from: 0=Checkbook; 1=Customer Default is zero on new record
        public string CashAcct { get; set; } //Cash account; only valid if DEFCACTY=1

        public string ReceivableAcct { get; set; }

        public string SalesAccnt { get; set; }

        public string CostofSalesAcct { get; set; }

        public string InventoryAcct { get; set; }

        public string TermsDicountTakeAcct { get; set; }

        public string TermsDicountAvailAcct { get; set; }

        public string FinanceChargesAcct { get; set; }

        public string WriteOffsAcct { get; set; }

        public string SalesOrderRetAcct { get; set; }

        public string OverPaymentWriteOffsAcct { get; set; }

        public string FrontOfficeItegrationId { get; set; }

        public short IntegrationSource { get; set; }

        public string IntegrationId { get; set; }

        public short UseCustomerClass { get; set; }

        //Flag to have class setting roll down to elements not passed in; uses the CUSTCLAS class to roll down
        public short CreateAddressCode { get; set; }

        public short RequesterTrx { get; set; }
    }
}