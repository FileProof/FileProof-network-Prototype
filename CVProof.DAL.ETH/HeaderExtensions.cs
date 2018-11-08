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
            header.Nonce = Utils.Convert.ToBytesString(RandomNonce.RandomBytes().Take(32).ToArray());
            header.DataHash = null;
        }

        public static string Serialize(this HeaderModel header)
        {            
            return JsonConvert.SerializeObject(header);
        }

        //These two functions used for calculating hash of Header object
        //GetFullHashBytes includes all fields in calculation, while GetSimpleHashBytes considers only two: Nonce and Datahash
        //This is done to decouple the key from the object and make it work separately
        //Otherwise including in the file data reference to its header would be impossible, leading to the infinite recursion.


        public static byte[] GetFullHashBytes(this HeaderModel header)
        {
            using (System.Security.Cryptography.HashAlgorithm sha256 = System.Security.Cryptography.SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(header.Serialize()));
            }            
        }

        public static byte[] GetSimpleHashBytes(this HeaderModel header)
        {
            HeaderModel simple = new HeaderModel()
            {
                Nonce = header.Nonce,
            };

            using (System.Security.Cryptography.HashAlgorithm sha256 = System.Security.Cryptography.SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(simple.Serialize()));
            }
        }
    }
}
