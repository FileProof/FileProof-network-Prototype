using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using CVProof.Models;

namespace CVProof.Web.Models
{
    public class ValidateViewModel
    {
        public List<IFormFile> Files { get; set; }
        public string Text { get; set; }

        public bool StoreFile { get; set; }

        public string Validator { get; set; }

        public string SubmitFile { get; set; }

        public string SubmitText { get; set; }

        public HeaderModel Header {get; set;}

    }
}
