using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CVProof.Models;
using CVProof.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CVProof.Web.Models
{
    public class CertificateViewModel : HeaderViewModel
    {
        public CertificateViewModel() { }

        public CertificateViewModel(HeaderModel model, IConfiguration config) : base(model, config)
        {
            this.BaseUrl = config["baseUrl"];
        }

        public List<IFormFile> Files { get; set; }       
        
        public bool NotFound { get; set; }

        public bool VerificationStatus { get { return this.HeaderId != null; } }        

        public string BaseUrl { get; set; }

    }
}
