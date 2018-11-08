using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CVProof.Models;

using Microsoft.AspNetCore.Http;

namespace CVProof.Web.Models
{
    public class VerifyViewModel
    {
        public List<IFormFile> Files { get; set; }
        public string Text { get; set; }

        public string SubmitFile { get; set; }

        public string SubmitText { get; set; }

        public HeaderModel Header { get; set; }

        public VerificationStatus Status { get; set; }

        public string HeaderId { get; set; }
    }
}
