using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace CVProof.Utils
{
    public static class Convert
    {
        public static string ToHexString(byte[] byteArray)
        {
            StringBuilder hex = new StringBuilder(byteArray.Length * 2);
            foreach (byte b in byteArray)
                hex.AppendFormat("{0:x2}", b);
            return "0x" + hex.ToString();
        }

        public static string ToHexString(string utf8String)
        {
            byte[] utf8Bytes = Encoding.UTF8.GetBytes(utf8String);

            return ToHexString(utf8Bytes);
        }

        public static string ReplaceUnicodeEscapeChars(string utf8String)
        {
            string ret = null;

            Regex rx = new Regex(@"\\[uU]([a-fA-F0-9]{4})");            
            ret = rx.Replace(utf8String, e => (Char.ConvertFromUtf32(Int32.Parse(e.Groups[1].Value, NumberStyles.HexNumber)).ToString()));

            return ret;
        }
    }
}
