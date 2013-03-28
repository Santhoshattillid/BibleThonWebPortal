using System.Configuration;
using AuthorizeNet;

namespace AlbaDL
{
    /// <summary>
    /// Summary description for Utilities
    /// </summary>
    public class Utilities
    {
        public static string _connString = ConfigurationManager.ConnectionStrings["GPConnectionString"].ToString();
        public const string SiteUrl = "";
        public const string ApiLoginID = "8hw54A7VJ";
        public const string TransactionKey = "7443fLVdSp89dU32";
        public const string MerchantHash = "C2D03ADC1E90F1221BA9";

        public static string AuthorizeNetUrl
        {
            get { return Gateway.TEST_URL; }
        }

        public static string GenerateFingerprint(decimal amount, string seq, string timeStamp)
        {
            return Crypto.GenerateFingerprint(TransactionKey, ApiLoginID, amount, seq, timeStamp);
        }

        public static bool DevelopmentMode = true;
    }
}