using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CVProof.Models;
using CVProof.Utils;


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

        public HeaderListFilters filters { get; set; }
    }


    public class HeaderListFilters
    {
        public string Id { get; set; }

        public string ValidatorId { get; set; }
    }
}

