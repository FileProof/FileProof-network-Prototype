using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CVProof.Models;


namespace CVProof.Web.Models
{
    public class HeaderListViewModel
    {
        public HeaderListViewModel() { }

        public HeaderListViewModel(IEnumerable<HeaderModel> headerList)
        {
            HeaderList = headerList.OrderBy(e => e.Timestamp).Select(e => new HeaderViewModel(e));
        }

        public IEnumerable<HeaderViewModel> HeaderList { get; set; }
    }

    public class HeaderViewModel
    {
        public HeaderViewModel() {}

        public HeaderViewModel(HeaderModel header)
        {
            this.ContainerVersion = header.ContainerVersion;
            //this.HeaderId = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(header.HeaderId));
            this.HeaderId = header.HeaderId;
            this.Category = header.Category;
            this.ValidatorName = header.ValidatorName;
            this.IssuerName = header.IssuerName;
            this.RecipientName = header.RecipientName;
            this.IssuerUuid = header.IssuerUuid;
            this.ValidatorUuid = header.ValidatorUuid;
            this.RecipientUuid = header.RecipientUuid;
            this.PreviousHeaderId = header.PreviousHeaderId;
            this.NextHeaderId = header.NextHeaderId;

            this.Timestamp = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Int32.Parse(header.Timestamp));
            this.BlockNumber = header.BlockNumber;
            this.DataAddress = header.DataAddress;
            this.ValidationExpiry = header.ValidationExpiry;
            this.ValidationCounter = header.ValidationCounter;
            this.DataHash = header.DataHash;
            this.Nonce = Utils.Convert.ToHexString(header.Nonce);
        }

        public string ContainerVersion { get; set; }
        public string HeaderId { get; set; }
        public string Category { get; set; }
        public string ValidatorName { get; set; }
        public string IssuerName { get; set; }
        public string RecipientName { get; set; }
        public string IssuerUuid { get; set; }
        public string ValidatorUuid { get; set; }
        public string RecipientUuid { get; set; }
        public string PreviousHeaderId { get; set; }
        public string NextHeaderId { get; set; }
        public DateTime Timestamp { get; set; }
        public string BlockNumber { get; set; }
        public string DataAddress { get; set; }
        public string ValidationExpiry { get; set; }
        public string ValidationCounter { get; set; }
        public string DataHash { get; set; }
        public string Nonce { get; set; }
    }
}

