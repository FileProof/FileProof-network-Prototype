using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.Extensions.Configuration;

using CVProof.Models;
using CVProof.Utils;

namespace CVProof.Web.Models
{
    public class HeaderViewModel
    {
        public HeaderViewModel(): this(null, null)
        {
        }

        public HeaderViewModel(HeaderModel header): this(header, null)
        {          
        }

        public HeaderViewModel(HeaderModel header, IConfiguration config)
        {
            if (config != null)
            {
                this.StorageBaseUrl = config["storageBaseUrl"];
                this.StorageBucket = config["storageBucket"];
            }

            if (header != null)
            {
                this.ContainerVersion = header.ContainerVersion;
                this.HeaderId = header.HeaderId;
                this.Category = header.Category;
                this.ValidatorName = header.ValidatorName;
                this.IssuerName = header.IssuerName;
                this.RecipientName = header.RecipientName;
                this.IssuerUuid = header.IssuerUuid;
                this.ValidatorUuid = header.ValidatorUuid;
                this.ValidatorLegitimationId = header.ValidatorLegitimationId;
                this.RecipientUuid = header.RecipientUuid;
                this.PreviousHeaderId = header.PreviousHeaderId;
                this.NextHeaderId = header.NextHeaderId;

                int ts = 0;

                this.Timestamp = String.IsNullOrEmpty(header.Timestamp) ? (DateTime?)null : new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Int32.TryParse(header.Timestamp, out ts) ? ts : 0);
                this.BlockNumber = header.BlockNumber;
                this.DataAddress = header.DataAddress;
                this.ValidationExpiry = header.ValidationExpiry;
                this.ValidationCounter = header.ValidationCounter;
                this.DataHash = header.DataHash;
                this.Nonce = String.IsNullOrEmpty(header.Nonce) ? header.Nonce : Utils.Convert.ToHexString(header.Nonce);
                this.Link = header.Stored ? header.HeaderId : null;
                this.Attachment = header.Attachment;
                this.GlobalHash = header.GlobalHash;
                this.ValidatorImage = header.ValidatorProfile?.Image != null ? System.Convert.ToBase64String(header.ValidatorProfile?.Image) : String.Empty;
                this.IssuerImage = header.IssuerProfile?.Image != null ? System.Convert.ToBase64String(header.IssuerProfile?.Image) : String.Empty;

                CategoryList = new List<DDLItem>() { new DDLItem() { ID = String.Empty, Desc = String.Empty } };

                foreach (string cat in Enum.GetNames(typeof(CVProof.Models.Category)))
                {
                    CategoryList.Add(new DDLItem() { ID = cat, Desc = cat });
                }
            }
        }

        #region HeaderModel proxy properties
        public string ContainerVersion { get; set; }
        public string HeaderId { get; set; }
        public string HeaderIdShortened { get { return String.Format("{0}", HeaderId.SubStringTo(7)); } }
        public string Category { get; set; }
        public string ValidatorName { get; set; }
        public string ValidatorNameShortened { get { return String.Format("{0}", ValidatorName.SubStringTo(7)); } }
        public string IssuerName { get; set; }
        public string RecipientName { get; set; }
        public string IssuerUuid { get; set; }
        public string ValidatorUuid { get; set; }
        public string ValidatorLegitimationId { get; set; }
        public string RecipientUuid { get; set; }
        public string PreviousHeaderId { get; set; }
        public string NextHeaderId { get; set; }
        public DateTime? Timestamp { get; set; }
        public string BlockNumber { get; set; }
        public string DataAddress { get; set; }
        public string DataAddressShortened { get { return String.Format("{0}", DataAddress.SubStringTo(7)); } }
        public string ValidationExpiry { get; set; }
        public string ValidationCounter { get; set; }
        public string DataHash { get; set; }
        public string DataHashShortened { get { return String.Format("{0}", DataHash.SubStringTo(7)); } }
        public string Nonce { get; set; }
        public string Link { get; set; }
        public string Attachment { get; set; }
        public string GlobalHash { get; set; }
        #endregion

        #region Viewmodel properties

        public string IssuerImage { get; set; }
        public string ValidatorImage { get; set; }
        public List<DDLItem> CategoryList { get; set; }
        public bool HasDelegation { get { return !String.IsNullOrEmpty(this.ValidatorLegitimationId); } }
        public bool HasAttachment { get { return !String.IsNullOrEmpty(this.Attachment); } }
        public string StorageBaseUrl { get; set; }
        public string StorageBucket { get; set; }
        #endregion

        #region Viewmodel methods
        public HeaderModel GetHeaderModel()
        {
            HeaderModel ret = new HeaderModel();
            
            ret.HeaderId = this.HeaderId;
            ret.Category = this.Category;
            ret.ValidatorName = this.ValidatorName;
            ret.IssuerName = this.IssuerName;
            ret.RecipientName = this.RecipientName;
            ret.IssuerUuid = this.IssuerUuid;
            ret.ValidatorUuid = this.ValidatorUuid;
            ret.ValidatorLegitimationId = this.ValidatorLegitimationId;
            ret.RecipientUuid = this.RecipientUuid;
            ret.PreviousHeaderId = this.PreviousHeaderId;
            ret.NextHeaderId = this.NextHeaderId;
            ret.Timestamp = Utils.Convert.ToTimestamp(this.Timestamp);
            ret.BlockNumber = this.BlockNumber;
            ret.DataAddress = this.DataAddress;
            ret.ValidationExpiry = this.ValidationExpiry;
            ret.ValidationCounter = this.ValidationCounter;
            ret.DataHash = this.DataHash;
            ret.Nonce = Utils.Convert.ToBytesString(this.Nonce);
            ret.Stored = !String.IsNullOrEmpty(this.Link);
            ret.Attachment = this.Attachment;
            ret.GlobalHash = this.GlobalHash;

            return ret;
        }
        #endregion

    }

}
