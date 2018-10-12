using System;

namespace CVProof.Models
{
    public class HeaderModel: IEquatable<HeaderModel>
    {
        public bool Equals(HeaderModel other)
        {
            if (other != null)
            {
                return this.DataHash.Equals(other.DataHash);
            }

            return false;
        }

        public string ContainerVersion { get { return "0.1.0"; } }
        public string HeaderId { get; set; }
        public string Category { get; set; }        
        public string ValidatorName { get; set; }
        public string IssuerName { get; set; }
        public string RecipientName { get; set; }
        public string IssuerUuid { get; set; }
        public string ValidatorUuid { get; set; }
        public string ValidatorLegitimationId { get; set; }
        public string RecipientUuid { get; set; }
        public string PreviousHeaderId { get; set; }
        public string NextHeaderId { get; set; }
        public string Timestamp { get; set; }
        public string BlockNumber { get; set; }
        public string DataAddress { get; set; }
        public string ValidationExpiry { get; set; }
        public string ValidationCounter { get; set; }
        public string DataHash { get; set; }
        public string Nonce { get; set; }
        public bool Stored { get; set; }
    }

    public class HashDto
    {
        public string hash { get; set; }
    }
}

