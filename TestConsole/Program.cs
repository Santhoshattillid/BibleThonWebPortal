using System;
using System.Linq;
using AlbaDL;
using TestConsole.ArbApiSoap;

namespace TestConsole
{
    internal class Program
    {
        public static Service _webservice = new Service();

        // This is the user's Payment Gateway account login name used for transaction processing.
        // You must give this a valid value to successfully execute this sample code.
        private static string _userLoginName = "79Rd7RcRWpwD";

        // This is the TransactionKey associated with that account.
        // You must give this a valid value to successfully execute this sample code.
        private static string _transactionKey = "56dzP6v6D2RG4xXB";

        // This will be set by CreateSubscription and used for the UpdateSubscription
        // and CancelSubscription
        private static long _subscriptionId = 0;

        private static void Main()
        {
            var twoEntities = new TWOEntities();
            var ord = new OrderDetail
            {
                CustomerName = "test",
                FormData = "Test",
                Operator = "Test",
                Status = "Test",
                Id = twoEntities.OrderDetails.Max(id => id.Id) + 1
            };

            twoEntities.OrderDetails.AddObject(ord);
            twoEntities.SaveChanges();

            // trying out the authorize service Automated Recurring Billing (ARB) API

            bool bResult;

            Console.WriteLine("Web service url: " + _webservice.Url);

            // Create a new subscription
            Console.WriteLine("\r\n\r\nThis call to ARBCreateSubscription should be successful.");
            bResult = CreateSubscription();

            // This example will generate an error because a duplicate subscription
            // is being created.
            Console.WriteLine("\r\n\r\nThis call to ARBCreateSubscription shows how to handle errors.");
            CreateSubscription();

            // Update the subscription that was just created
            if (bResult && _subscriptionId > 0)
            {
                Console.WriteLine("\r\n\r\nThis call to ARBUpdateSubscription should be successful.");
                bResult = UpdateSubscription();

                // Cancel the subscription that was just created
                if (bResult)
                {
                    Console.WriteLine("\r\n\r\nThis call to ARBCancelSubscription should be successful.");

                    // bResult = CancelSubscription();
                }

                // Get the status of the subscription we just canceled
                bResult = GetStatusSubscription();
            }

            Console.WriteLine("\r\nPress any key to exit.");
            Console.ReadKey();
        }

        private static bool CreateSubscription()
        {
            bool bResult = true;

            Console.WriteLine("\r\nCreate subscription");

            MerchantAuthenticationType authentication = PopulateMerchantAuthentication();

            var subscription = new ARBSubscriptionType();
            PopulateSubscription(subscription, false);

            ARBCreateSubscriptionResponseType response = _webservice.ARBCreateSubscription(authentication, subscription);

            if (response.resultCode == MessageTypeEnum.Ok)
            {
                _subscriptionId = response.subscriptionId;
                Console.WriteLine("A subscription with an ID of '{0}' was successfully created.", _subscriptionId);
            }
            else
            {
                bResult = false;
                WriteErrors(response);
            }

            return bResult;
        }

        // ----------------------------------------------------------------------------------------
        /// <summary>
        /// Get the status of an existing ARB subscription
        /// </summary>
        // ----------------------------------------------------------------------------------------
        private static void PopulateSubscription(ARBSubscriptionType sub, bool bForUpdate)
        {
            var creditCard = new CreditCardType();

            sub.name = "Sample subscription";

            creditCard.cardNumber = "4111111111111111";

            creditCard.expirationDate = bForUpdate ? "2029-07" : "2029-07";

            sub.payment = new PaymentType
                              {
                                  Item = creditCard
                              };

            sub.customer = new CustomerType
                               {
                               };

            sub.billTo = new NameAndAddressType
                             {
                                 firstName = "Siva",
                                 lastName = "Sha"
                             };

            // Create a subscription that is 12 monthly payments starting on Jan 1, 2019

            sub.paymentSchedule = new PaymentScheduleType
                                      {
                                          startDate = new DateTime(2013, 3, 20),
                                          startDateSpecified = true,
                                          totalOccurrences = 5,
                                          totalOccurrencesSpecified = true
                                      };

            sub.amount = 10.00M;
            sub.amountSpecified = true;

            if (!bForUpdate)
            { // Interval can't be updated once a subscription is created.
                sub.paymentSchedule.interval = new PaymentScheduleTypeInterval
                                                   {
                                                       length = 1,
                                                       unit = ARBSubscriptionUnitEnum.months
                                                   };
            }
        }

        private static MerchantAuthenticationType PopulateMerchantAuthentication()
        {
            var authentication = new MerchantAuthenticationType
                                     {
                                         name = _userLoginName,
                                         transactionKey = _transactionKey,
                                     };
            return authentication;
        }

        private static void WriteErrors(ANetApiResponseType response)
        {
            Console.WriteLine("The API request failed with the following errors:");
            foreach (MessagesTypeMessage t in response.messages)
            {
                Console.WriteLine("[" + t.code
                                  + "] " + t.text);
            }
        }

        private static bool CancelSubscription()
        {
            bool bResult = true;

            Console.WriteLine("\r\nCancel subscription");

            MerchantAuthenticationType authentication = PopulateMerchantAuthentication();

            ARBCancelSubscriptionResponseType response;
            response = _webservice.ARBCancelSubscription(authentication, _subscriptionId);

            if (response.resultCode == MessageTypeEnum.Ok)
            {
                Console.WriteLine("The subscription was successfully cancelled.");
            }
            else
            {
                bResult = false;
                WriteErrors(response);
            }

            return bResult;
        }

        private static bool GetStatusSubscription()
        {
            bool bResult = true;

            Console.WriteLine("\r\nGet subscription status");

            MerchantAuthenticationType authentication = PopulateMerchantAuthentication();

            ARBGetSubscriptionStatusResponseType response = _webservice.ARBGetSubscriptionStatus(authentication, _subscriptionId);

            if (response.resultCode == MessageTypeEnum.Ok)
            {
                Console.WriteLine("Status Text: " + response.status + "\r\n");
            }
            else
            {
                bResult = false;
                WriteErrors(response);
            }

            return bResult;
        }

        private static bool UpdateSubscription()
        {
            bool bResult = true;

            Console.WriteLine("\r\nUpdate subscription");

            MerchantAuthenticationType authentication = PopulateMerchantAuthentication();

            var subscription = new ARBSubscriptionType();
            PopulateSubscription(subscription, true); // Expiration date will be different.

            ARBUpdateSubscriptionResponseType response = _webservice.ARBUpdateSubscription(authentication, _subscriptionId, subscription);

            if (response.resultCode == MessageTypeEnum.Ok)
            {
                Console.WriteLine("The subscription was successfully updated.");
            }
            else
            {
                bResult = false;
                WriteErrors(response);
            }

            return bResult;
        }
    }
}