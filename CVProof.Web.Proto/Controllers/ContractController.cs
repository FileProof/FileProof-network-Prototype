using CVProof.DAL.ETH;
using CVProof.DAL.SQL;
using CVProof.DAL.AWS;
using CVProof.Export.PDF;
using CVProof.Models;
using CVProof.Utils;
using CVProof.Web.Models;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http;

namespace CVProof.Web.Controllers
{
    public class ContractController : BaseController
    {
        public ContractController(IConfiguration configuration, IUserMgr user) : base(configuration, user){}

        public IActionResult Validate()
        {
            ValidateViewModel model = new ValidateViewModel();

            if (_user.IsAuthenticated) { ViewBag.User = _user.User; }
            ViewData["Title"] = "Validate";

            return View(model);
        }
        
        [HttpPost]
        public async Task SaveText(ValidateViewModel model)
        {
            HeaderModel header = new HeaderModel();            
            header.Init();
            header.ValidatorUuid = model.Validator;
            header.ValidatorName = model.Validator;
            header.Category = Category.Text.ToString();

            if (!String.IsNullOrEmpty(model.Text))
            {
                using (System.Security.Cryptography.HashAlgorithm sha256 = System.Security.Cryptography.SHA256.Create())
                {                  
                    header.DataHash = Utils.Convert.ToHexString(sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(model.Text)));
                }

                header.HeaderId = Utils.Convert.ToHexString(header.GetFullHashBytes());

                SQLData.InsertHeader(header);
            }            
        }


        [HttpPost]
        public async Task SaveFile(ValidateViewModel model)
        {           
            HeaderModel header = new HeaderModel();
            //header.ValidatorUuid = model.Validator;
            header.ValidatorName = model.Validator;
            header.IssuerName = model.Validator;
            header.Category = Category.File.ToString();
            header.Stored = model.StoreFile;            

            try
            {
                foreach (var file in model.Files)
                {
                    header.Init();
                    header.HeaderId = Utils.Convert.ToHexString(header.GetSimpleHashBytes());

                    if (file != null && file.Length > 0)
                    {
                        byte[] sourceFile;                        

                        using (MemoryStream stream = new MemoryStream())
                        {
                            await file.CopyToAsync(stream);
                            sourceFile = stream.ToArray();
                        }

                        if (model.StoreFile)
                        {
                            byte[] stampedFile;

                            try
                            {                                    
                                stampedFile = await GetPDF(sourceFile, header.HeaderId);
                            }
                            catch(Exception e)
                            {
                                stampedFile = sourceFile;
                            }

                            using (System.Security.Cryptography.HashAlgorithm sha256 = System.Security.Cryptography.SHA256.Create())
                            {
                                header.DataHash = Utils.Convert.ToHexString(sha256.ComputeHash(stampedFile));
                            }

                            using (MemoryStream stream = new MemoryStream(stampedFile))
                            {                                                                        
                                await S3.WriteObject(stream, "cvproof", header.HeaderId, file.ContentType);
                            }

                        }
                        else
                        {
                            using (System.Security.Cryptography.HashAlgorithm sha256 = System.Security.Cryptography.SHA256.Create())
                            {
                                header.DataHash = Utils.Convert.ToHexString(sha256.ComputeHash(sourceFile));
                            }                                
                        }                        

                        header.GlobalHash = Utils.Convert.ToHexString(header.GetFullHashBytes());

                        SQLData.InsertHeader(header);                      
                    }
                }
            }

            catch (Exception e)
            {
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<HeaderViewModel> Validate([FromBody] HashDto dto)
        {
            HeaderModel header = SQLData.GetHeaderById(dto.hash);

            header.ValidatorUuid = _user.User;
            if (String.IsNullOrEmpty(header.ValidatorLegitimationId) || String.IsNullOrEmpty(header.IssuerUuid))
            {
                header.IssuerUuid = _user.User;
            }

            header.ValidationCounter = "1";

            Ethereum eth = new Ethereum();

            header = await eth.SendToNetwork(header);

            SQLData.UpdateHeader(header);

            return new HeaderViewModel(header);            
        }

        [HttpPost]
        public void DeleteAll()
        {
            SQLData.DeleteAll();
        }

        public async Task<IActionResult> Verify(VerifyViewModel model = null)
        {
            if (_user.IsAuthenticated) { ViewBag.User = _user.User; }
            ViewData["Title"] = "Verify";

            return View(model ?? new VerifyViewModel());
        }

        [HttpPost]
        public async Task<bool> VerifyText(VerifyViewModel model)
        {
            byte[] datahash;

            HeaderModel header = new HeaderModel();     
            
            if (!String.IsNullOrEmpty(model.Text))
            {
                using (System.Security.Cryptography.HashAlgorithm sha256 = System.Security.Cryptography.SHA256.Create())
                {
                    datahash = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(model.Text));
                }

                var datahashString = Utils.Convert.ToHexString(datahash);//System.Text.Encoding.UTF8.GetString(datahash);

                model.Status = SQLData.GetHeaderByData(datahashString) != null ? VerificationStatus.True : VerificationStatus.False;               
            }


            return model.Status == VerificationStatus.True;
        }


        [HttpPost]
        public async Task<bool> VerifyFile(VerifyViewModel model)
        {
            byte[] datahash;

            try
            {
                foreach (var file in model.Files)
                {
                    datahash = null;

                    if (file != null && file.Length > 0)
                    {
                        using (var stream = file.OpenReadStream())
                        {                       
                            using (System.Security.Cryptography.HashAlgorithm sha256 = System.Security.Cryptography.SHA256.Create())
                            {
                                datahash = sha256.ComputeHash(stream);
                            }
                        }

                        var datahashString = Utils.Convert.ToHexString(datahash);
                       
                        model.Status = SQLData.GetHeaderByData(datahashString) != null ? VerificationStatus.True : VerificationStatus.False;
                    }
                }
            }

            catch (FileNotFoundException e)
            {
                return model.Status == VerificationStatus.None;
            }

            catch (IOException e)
            {
                return model.Status == VerificationStatus.None;
            }

            return model.Status == VerificationStatus.True;
        }

        [HttpPost]
        public async Task<bool> VerifyCertificateFile(VerifyViewModel model)
        {
            bool ret = false;

            byte[] datahash;

            try
            {
                datahash = null;

                var file = model.Files.FirstOrDefault();
                    
                if (file != null && file.Length > 0)
                {
                    using (var stream = file.OpenReadStream())
                    {
                        using (System.Security.Cryptography.HashAlgorithm sha256 = System.Security.Cryptography.SHA256.Create())
                        {
                            datahash = sha256.ComputeHash(stream);
                        }
                    }

                    var datahashString = Utils.Convert.ToHexString(datahash);

                    HeaderModel header = SQLData.GetHeaderByData(datahashString);

                    ret = (header != null) && (header.HeaderId == model.HeaderId);                    
                }
            }

            catch (Exception e)
            {
                ret = false;
            }

            return ret;
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult ShowDocs(HeaderListFilters filters = null)
        {
            if (_user.IsAuthenticated) { ViewBag.User = _user.User; }
            ViewData["Title"] = "Documents";

            IEnumerable<HeaderModel> hdrList = SQLData.GetHeaders();

            if (filters != null)
            {
                if (!string.IsNullOrEmpty(filters.Id)) { hdrList = hdrList.Where(e => String.Compare(e.HeaderId, filters.Id) == 0); }
                if (!string.IsNullOrEmpty(filters.ValidatorId)) { hdrList = hdrList.Where(e => String.Compare(e.ValidatorUuid, filters.ValidatorId) == 0); }
            }

            return View(new HeaderListViewModel(hdrList));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult EditDoc(string id)
        {
            if (_user.IsAuthenticated) { ViewBag.User = _user.User; }
            ViewData["Title"] = "Documents";

            HeaderModel header = SQLData.GetHeaderById(id);

            if (!String.IsNullOrEmpty(header.ValidationCounter))
                return RedirectToAction("ShowDocs");

            return View(new HeaderViewModel(header));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public IActionResult SaveDoc(HeaderViewModel model)
        {
            if (_user.IsAuthenticated) { ViewBag.User = _user.User; }
            ViewData["Title"] = "Documents";

            if (!String.IsNullOrEmpty(model.ValidationCounter))
                return RedirectToAction("ShowDocs");

            SQLData.UpdateHeader(model.GetHeaderModel());

            return RedirectToAction("ShowDocs");
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ActionResult Certificate(string id)
        {
            if (_user.IsAuthenticated) { ViewBag.User = _user.User; }
            ViewData["Title"] = "Certificate";

            HeaderModel header = SQLData.GetHeaderById(id);

            return View(new HeaderViewModel(header));
        }

        public ActionResult CertificateSimple(string id)
        {
            if (_user.IsAuthenticated) { ViewBag.User = _user.User; }
            ViewData["Title"] = "Certificate";

            HeaderModel header = SQLData.GetHeaderById(id);

            return View(new HeaderViewModel(header));
        }

        private async Task<byte[]> GetPDF(byte[] file, string id)
        {
            byte[] ret = null;

            string inputFilename = "./cert.html";            

            HtmlDto dto = new HtmlDto()
            {
                html = System.Web.HttpUtility.HtmlEncode(
                            Encoding.UTF8.GetString(
                                System.IO.File.ReadAllBytes(inputFilename)))
                                .Replace("{certificateid}", id),
                footerPath = "http://token-certificate.fileproof.org/footer.html"
            };

            string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(dto);

            string uri = @"http://cvproofapi.cloudapp.net:8080/api/Pdf/GeneratePDF";

            //var values = new Dictionary<string, string>
            //                {
            //                   { "html", query2 }
            //                };

            //var content = new FormUrlEncodedContent(values);

            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{uri}", content).ConfigureAwait(false);

            var file2 = await response.Content.ReadAsByteArrayAsync();

            ret = Concat.DoConcat(file, file2);
            
            return ret;

        }
    }
}
