using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using CVProof.Models;
using CVProof.Utils;


namespace CVProof.Web.Models
{
    public class HeaderListViewModel
    {
        public HeaderListViewModel() : this(null) { }

        public HeaderListViewModel(IEnumerable<HeaderModel> headerList) : this (headerList, null) { }

        public HeaderListViewModel(IEnumerable<HeaderModel> headerList, IConfiguration config)
        {
            if (headerList != null)
                HeaderList = headerList.OrderBy(e => e.Timestamp).Select(e => new HeaderViewModel(e));

            if (config != null)
            {
                this.StorageBaseUrl = config["storageBaseUrl"];
                this.StorageBucket = config["storageBucket"];
            }
        }

        public IEnumerable<HeaderViewModel> HeaderList { get; set; }

        public HeaderListFilters filters { get; set; }

        public string StorageBaseUrl { get; set; }

        public string StorageBucket { get; set; }
    }


    public class HeaderListFilters
    {
        public string Id { get; set; }

        public string ValidatorId { get; set; }
    }
}

