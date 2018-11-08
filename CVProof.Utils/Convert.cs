using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace CVProof.Utils
{
    public static class Convert
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

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

        public static byte[] ToBytes(string hexString)
        {
            string s = hexString.Substring(2);
            int NumberChars = s.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = System.Convert.ToByte(s.Substring(i, 2), 16);
            return bytes;
        }

        public static string ToBytesString(string hexString)
        {
            byte[] utf8Bytes = ToBytes(hexString);

            return Encoding.UTF8.GetString(utf8Bytes);
        }

        public static string ToBytesString(byte[] byteArray)
        {
            return Encoding.UTF8.GetString(byteArray);
        }

        public static string ReplaceUnicodeEscapeChars(string utf8String)
        {
            string ret = null;

            Regex rx = new Regex(@"\\[uU]([a-fA-F0-9]{4})");
            ret = rx.Replace(utf8String, e => (Char.ConvertFromUtf32(Int32.Parse(e.Groups[1].Value, NumberStyles.HexNumber)).ToString()));

            return ret;
        }        

        public static string ToTimestamp(DateTime? value)
        {
            string ret = String.Empty;

            if (value != null)
            {                
                TimeSpan elapsedTime = value.Value - Epoch;
                long timeStamp = (long)elapsedTime.TotalSeconds;
                ret = timeStamp.ToString();
            }

            return ret;
        }
    }
}
