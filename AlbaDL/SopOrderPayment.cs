namespace AlbaDL
{
    public class SopOrderPayment
    {
        public short SOPTYPE { get; set; }

        public string SOPNUMBE { get; set; }

        public string CUSTNMBR { get; set; }

        public string DOCDATE { get; set; }

        public decimal DOCAMNT { get; set; } //Payment amount; required and used only when Action=1 or 3

        public string CHEKBKID { get; set; }

        public string CARDNAME { get; set; } //Credit card name--value if credit card transaction

        public string CHEKNMBR { get; set; } //Check number--value if check

        public string RCTNCCRD { get; set; } //Credit card number--value if credit card

        public string AUTHCODE { get; set; } //Authorization code--value if credit card

        public string EXPNDATE { get; set; } //Credit card expiration date

        public short PYMTTYPE { get; set; } //1=Cash deposit; 2=Check deposit; 3=Credit card deposit; 4=Cash payment; 5=Check payment; 6=Credit card payment

        public string DOCNUMBR { get; set; } //Payment number/cash receipt number; required if Action=2, SOPTYPE=2, and PYMTTYPE=(1, 2, or 3); required if Action=2 and SOPTYPE=3; required if Action=3, SOPTYPE=2, and PYMTTYPE=(1, 2, or 3)

        public short Action { get; set; } //Default from setup: 1=New transaction/new payment;3=Modify existing paymentMust set to 2 manually; 2=Delete payment

        public short SEQNUMBR { get; set; } //Sequence number; required if Action=1, SOPTYPE=2, and multiple PYMTTYPE=6 exist Required if Action=2, SOPTYPE=2, and PYMTTYPE=6 Required if Action=3 and DOCNUMBR and PYMTTYPE elements exist multiple times on document

        public string MDFUSRID { get; set; }

        public string VOIDDATE { get; set; }

        public short PROCESSELECTRONICALLY { get; set; } //Process Electronically 0 = False, 1 = True
    }
}