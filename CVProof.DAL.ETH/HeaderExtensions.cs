using System.Linq;
using System.Collections.Generic;
using System.Text;
using CVProof.Models;
using CVProof.Utils;
using Newtonsoft.Json;

namespace CVProof.DAL.ETH
{
    public static class HeaderExtensions
    {
        public static void Init(this HeaderModel header)
        {            
            header.Nonce = Encoding.UTF8.GetString(RandomNonce.RandomBytes().Take(32).ToArray());
            header.DataHash = null;
        }

        public static string Serialize(this HeaderModel header)
        {            
            return JsonConvert.SerializeObject(header);
        }

        public static byte[] GetHashBytes(this HeaderModel header)
        {
            using (System.Security.Cryptography.HashAlgorithm sha256 = System.Security.Cryptography.SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(header.Serialize()));
            }            
        }        
    }
}
