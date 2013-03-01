using System;
using System.Linq;
using System.Security.Cryptography;
using System.IO;
using System.Text;

/// <summary>
/// Summary description for clsCrypto
/// </summary>

public class clsCrypto
{
    public static string EncryptString(string pClearText)
    {

        byte[] clearTextBytes = Encoding.UTF8.GetBytes(pClearText);

        System.Security.Cryptography.SymmetricAlgorithm rijn = SymmetricAlgorithm.Create();

        MemoryStream ms = new MemoryStream();
        byte[] rgbIV = Encoding.ASCII.GetBytes("ryxjvlzmdalyglrj");
        byte[] key = Encoding.ASCII.GetBytes("hcxilkqbbhczfeultgbskdmaunivmfuo");
        CryptoStream cs = new CryptoStream(ms, rijn.CreateEncryptor(key, rgbIV), CryptoStreamMode.Write);

        cs.Write(clearTextBytes, 0, clearTextBytes.Length);

        cs.Close();

        return Convert.ToBase64String(ms.ToArray());
    }

    public static string DecryptString(string pEncryptedText)
    {
        byte[] encryptedTextBytes = Convert.FromBase64String(pEncryptedText);

        MemoryStream ms = new MemoryStream();

        System.Security.Cryptography.SymmetricAlgorithm rijn = SymmetricAlgorithm.Create();


        byte[] rgbIV = Encoding.ASCII.GetBytes("ryxjvlzmdalyglrj");
        byte[] key = Encoding.ASCII.GetBytes("hcxilkqbbhczfeultgbskdmaunivmfuo");

        CryptoStream cs = new CryptoStream(ms, rijn.CreateDecryptor(key, rgbIV),
        CryptoStreamMode.Write);

        cs.Write(encryptedTextBytes, 0, encryptedTextBytes.Length);

        cs.Close();

        return Encoding.UTF8.GetString(ms.ToArray());

    }
}
