using System;
using AlbaBL.AuthorizeNetWebSvc;
using AlbaDL;

namespace AlbaBL
{
    public class AuthorizeNetSubscription
    {
        public long CreateSubscription(CardDetails cardDetails, DateTime subscriptionStartDate, short noOfOccurence, decimal amount, short startDayOfMonth, string customerNumber, string customerName, string phoneNumber)
        {
            // initiating Athorize.Net WebSvc client
            var serviceClient = new Service();

            // defining merchant authentication
            var authentication = new MerchantAuthenticationType
                                     {
                                         name = Utilities.ApiLoginID,
                                         transactionKey = Utilities.TransactionKey,
                                     };

            // defining credit card details
            var creditCard = new CreditCardType
                                 {
                                     cardNumber = cardDetails.CardNo,
                                     expirationDate = cardDetails.ExpireDate.ToString("yyyy-MM"),
                                     cardCode = cardDetails.CardCode
                                 };

            var subscriptionType = new ARBSubscriptionType
                                       {
                                           name = "ShareAThonSubscription",
                                           payment = new PaymentType { Item = creditCard },
                                           customer = new CustomerType
                                                          {
                                                              id = customerNumber,
                                                              phoneNumber = phoneNumber,
                                                              type = CustomerTypeEnum.individual,
                                                              typeSpecified = true
                                                          },
                                           billTo = new NameAndAddressType
                                                        {
                                                            firstName = customerName
                                                        },
                                           paymentSchedule = new PaymentScheduleType
                                                                 {
                                                                     startDate = subscriptionStartDate,
                                                                     startDateSpecified = true,
                                                                     totalOccurrences = noOfOccurence,
                                                                     totalOccurrencesSpecified = true,
                                                                     interval = new PaymentScheduleTypeInterval
                                                                                    {
                                                                                        length = startDayOfMonth,
                                                                                        unit = ARBSubscriptionUnitEnum.months
                                                                                    }
                                                                 },
                                           amount = amount,
                                           amountSpecified = true
                                       };

            ARBCreateSubscriptionResponseType response = serviceClient.ARBCreateSubscription(authentication, subscriptionType);

            if (response.resultCode == MessageTypeEnum.Ok)
            {
                return response.subscriptionId;
            }
            return 0;
        }
    }
}