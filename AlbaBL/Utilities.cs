using System.Configuration;
using AuthorizeNet;

namespace AlbaBL
{
    /// <summary>
    /// Summary description for Utilities
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// connection string for GP database
        /// </summary>
        public static string _connString = ConfigurationManager.ConnectionStrings["GPConnectionString"].ToString();

        /// <summary>
        /// SiteUrl used for Authorize.net redirection
        /// </summary>
        public static string SiteUrl
        {
            get
            {
                return DevelopmentMode ? "http://localhost:8804" : "";
            }
        }

        /// <summary>
        /// Authorize.Net API Login ID
        /// </summary>
        public const string ApiLoginID = "8hw54A7VJ";

        /// <summary>
        /// Authorize.Net API Transaction Key
        /// </summary>
        public const string TransactionKey = "7443fLVdSp89dU32";

        /// <summary>
        /// Authorize.Net API Merchant Hash Key
        /// </summary>
        public const string MerchantHash = "C2D03ADC1E90F1221BA9";

        /// <summary>
        /// Authorize.Net Post URL
        /// </summary>
        public static string AuthorizeNetUrl
        {
            get
            {
                return DevelopmentMode ? Gateway.TEST_URL : Gateway.LIVE_URL;
                //return Gateway.TEST_URL;
                //return "http://localhost:1632/Receiver.aspx";
            }
        }

        /// <summary>
        /// For generating hash for Authorize.net
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="seq"></param>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static string GenerateFingerprint(decimal amount, string seq, string timeStamp)
        {
            return Crypto.GenerateFingerprint(TransactionKey, ApiLoginID, amount, seq, timeStamp);
        }

        /// <summary>
        /// Mode of defining development
        /// </summary>
        public const bool DevelopmentMode = true;

        /// <summary>
        /// Reference Customer Name for getting master details from GP
        /// </summary>
        public const string CustomerReferenceNameForAccounts = "ADAMPARK0001";
    }
}