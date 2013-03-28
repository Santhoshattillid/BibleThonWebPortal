namespace AlbaDL
{
    /// <summary>
    /// Customer Address class defines all attributes
    /// </summary>
    public class CustomerAddress
    {
        public string CustomerNumber { get; set; }

        public string AddressCode { get; set; }

        public string SalesPersonId { get; set; }

        public string UpsZone { get; set; }

        public string ContactPerson { get; set; }

        public string ShippingMethod { get; set; }

        public string TaxScheduleId { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Address3 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public string Country { get; set; }

        public string PhoneNumber1 { get; set; }

        public string PhoneNumber2 { get; set; }

        public string PhoneNumber3 { get; set; }

        public string Fax { get; set; }

        public string FrontOfficeIntegrationId { get; set; }

        public short IntegrationSource { get; set; }

        public string IntegrationId { get; set; }

        public string CountryCode { get; set; }

        public string LocationCode { get; set; }

        public string SalesTerritorry { get; set; }

        public string ShipToName { get; set; }

        public string PrintPhoneNumber { get; set; } //Print phone number:0=Do not print;1=Phone 1;2=Phone 2;3=Phone 3;4=Fax

        public short UpdateIfExists { get; set; }
    }
}