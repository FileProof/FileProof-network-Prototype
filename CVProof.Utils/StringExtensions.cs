using System;
using System.Collections.Generic;
using System.Text;

namespace CVProof.Utils
{
    public static class StringExtensions
    {
        public static string SubStringTo(this string s, int limit)
        {

            if (s.Length > limit)
            {
                return $"{s.Substring(0, limit)}...";
            }
            return s;

        }
    }
}
