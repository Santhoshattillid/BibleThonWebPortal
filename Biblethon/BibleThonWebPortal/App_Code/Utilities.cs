
using AuthorizeNet;

/// <summary>
/// Summary description for Utilities
/// </summary>
public class Utilities
{
    public static string SiteUrl = "";
    public static string ApiLoginID = "8hw54A7VJ";
    public static string TransactionKey = "7443fLVdSp89dU32";
    public static string MerchantHash = "C2D03ADC1E90F1221BA9";

    public static string AuthorizeNetUrl
    {
        get { return Gateway.TEST_URL; }
    }

    public static string GenerateFingerprint(decimal amount,string seq,string timeStamp)
    {
        return Crypto.GenerateFingerprint(TransactionKey, ApiLoginID, amount, seq, timeStamp);
    }
    
}