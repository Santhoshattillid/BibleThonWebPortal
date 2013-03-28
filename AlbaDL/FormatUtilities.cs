namespace AlbaDL
{
    public static class FormatUtilities
    {
        public static string FormatToTelephone(string phrase)
        {
            if (!string.IsNullOrEmpty(phrase) && phrase.Trim() != "00000000000000")
            {
                string outPut = string.Empty;
                if (phrase.Length > 3)
                    outPut = "(" + phrase.Substring(0, 3) + ") ";
                if (phrase.Length > 6)
                    outPut += phrase.Substring(3, 3) + "-";
                if (phrase.Length > 10)
                    outPut += phrase.Substring(6, 4) + " Ext. ";
                if (phrase.Length > 13)
                    outPut += phrase.Substring(10);
                return outPut;
            }
            return string.Empty;
        }

        public static string FormatToText(string phrase)
        {
            return phrase.Replace("(", "").Replace(")", "").Replace("Ext.", "").Replace(" ", "").Replace("-", "");
        }

        public static decimal ConvertToDecimal(string value)
        {
            return decimal.Parse(value.Replace("$", ""));
        }
    }
}