using System.Collections.Generic;
using AlbaDL;

namespace AlbaBL
{
    public class CustomerProcess
    {
        /// <summary>
        /// Creates the customer.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="customer">The customer.</param>
        /// <param name="listcustomerAddressInet">The listcustomer address inet.</param>
        /// <param name="isCustomerCreated">if set to <c>true</c> [is customer created].</param>
        private void CreateCustomer(string fileName, CustomerDetails customer, List<CustomerInetInfo> listcustomerAddressInet, out bool isCustomerCreated)
        {
            isCustomerCreated = new EConnectModel().SerilizeCustomerObject(fileName, Utilities._connString, customer, listcustomerAddressInet);
        }

        /// <summary>
        /// Creates the customer.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="customer">The customer.</param>
        /// <param name="listcustomerAddressInet">The listcustomer address inet.</param>
        /// <param name="importAccountsFromReferenceCustomer"> </param>
        /// <param name="isCustomerCreated">if set to <c>true</c> [is customer created].</param>
        public void CreateCustomer(string fileName, NewCustomer customer, List<CustomerInetInfo> listcustomerAddressInet, bool importAccountsFromReferenceCustomer, out bool isCustomerCreated)
        {
            isCustomerCreated = false;
            if (importAccountsFromReferenceCustomer)
            {
                var econnectModel = new EConnectModel();

                //var referencedCustomer = econnectModel.GetCustomerInformation(Utilities._connString, Utilities.CustomerReferenceNameForAccounts);
                var customerAccount = econnectModel.GetCustomerAccounts(Utilities._connString, Utilities.CustomerReferenceNameForAccounts, customer);
                var customerAddress = econnectModel.GtCustomerAddress(Utilities._connString, Utilities.CustomerReferenceNameForAccounts);

                if (!UserExists(customer.CustomerNumber))
                    isCustomerCreated = econnectModel.SerilizeCustomerObject(fileName, Utilities._connString, customerAccount, customerAddress, customer.CustomerName.Trim(), customer.CustomerNumber.Trim());
            }

            else
            {
                var customerDetails = new CustomerDetails
                {
                    Address1 = customer.Address1,
                    Address2 = customer.Address2,
                    Address3 = customer.Address3,
                    AddressCode = customer.AddressCode,
                    BankName = customer.BankName,
                    City = customer.City,
                    ContactPerson = customer.ContactPerson,
                    Country = customer.Country,
                    CountryCode = customer.CountryCode,
                    CreditCardId = customer.CreditCardId,
                    CreditCardNumber = customer.CreditCardNumber,
                    CreditExpiryDate = customer.CreditExpiryDate,
                    CurrencyId = customer.CurrencyId,
                    CustomerName = customer.CustomerName,
                    CustomerNumber = customer.CustomerNumber,
                    CustShortName = customer.CustShortName,
                    Fax = customer.Fax,
                    Hold = customer.Hold,
                    InActive = customer.InActive,
                    PhoneNumber1 = customer.PhoneNumber1,
                    PhoneNumber2 = customer.PhoneNumber2,
                    PhoneNumber3 = customer.PhoneNumber3,
                    State = customer.State,
                    StatementName = customer.StatementName,
                    UpdateIfExists = customer.UpdateIfExists,
                    ZipCode = customer.ZipCode
                };
                CreateCustomer(fileName, customerDetails, listcustomerAddressInet, out isCustomerCreated);
            }
        }

        /// <summary>
        /// Checks whether the customer is exists or not with customer number
        /// </summary>
        /// <param name="customerNumber"></param>
        /// <returns></returns>
        public bool UserExists(string customerNumber)
        {
            return new EConnectModel().CheckCustomer(Utilities._connString, customerNumber);
        }
    }
}