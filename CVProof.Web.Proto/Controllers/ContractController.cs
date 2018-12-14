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

        [AllowAnonymous]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public IActionResult Validate()
        {
            if (_user.IsAuthenticated) { ViewBag.User = _user.User; ViewBag.Roles = _user.Roles; }

            ValidateViewModel model = new ValidateViewModel();
            
            ViewData["Title"] = "Validate";

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task SaveFile(ValidateViewModel model)
        {           
            HeaderModel header = new HeaderModel();
            
            header.IssuerUuid = _user.User;
            header.IssuerName = SQLData.GetProfileById(_user.User).Name;
            
            if (String.IsNullOrEmpty(model.Validator))
            {
                header.ValidatorUuid = header.IssuerUuid;
                header.ValidatorName = header.IssuerName;
            }
            else
            {
                header.ValidatorName = model.Validator;              
            }

            header.Category = Category.File.ToString();
            header.Stored = model.StoreFile;            

            try
            {
                var file = model.Files[0];
                var attachment = model.Attachment;
                
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
                        #region stamping

                        byte[] stampedFile;

                        try
                        {                                    
                            stampedFile = await GetPDF(sourceFile, header.HeaderId);
                        }
                        catch(Exception e)
                        {
                            stampedFile = sourceFile;
                        }

                        #endregion

                        #region hash
                        using (System.Security.Cryptography.HashAlgorithm sha256 = System.Security.Cryptography.SHA256.Create())
                        {
                            header.DataHash = Utils.Convert.ToHexString(sha256.ComputeHash(stampedFile));
                        }
                        #endregion

                        #region store
                        using (MemoryStream stream = new MemoryStream(stampedFile))
                        {
                            S3.Init(_configuration["accessKey"], _configuration["secretKey"], _configuration["storageBucket"]);
  
                            await S3.WriteObject(stream, header.HeaderId, file.ContentType);
                        }
                        #endregion

                        if (model.Attachment != null)
                        {
                            byte[] attachmentFile;

                            using (MemoryStream stream = new MemoryStream())
                            {
                                await attachment.CopyToAsync(stream);
                                attachmentFile = stream.ToArray();
                            }

                            #region init
                            HeaderModel attachmentHeader = new HeaderModel();
                            attachmentHeader.Init();
                            attachmentHeader.HeaderId = Utils.Convert.ToHexString(attachmentHeader.GetSimpleHashBytes());
                            attachmentHeader.ValidatorName = model.Validator;
                            attachmentHeader.IssuerName = model.Validator;
                            attachmentHeader.IssuerUuid = _user.User;
                            attachmentHeader.Category = Category.Attachment.ToString();                            
                            attachmentHeader.Stored = true;
                            #endregion

                            #region hash
                            using (System.Security.Cryptography.HashAlgorithm sha256 = System.Security.Cryptography.SHA256.Create())
                            {
                                attachmentHeader.DataHash = Utils.Convert.ToHexString(sha256.ComputeHash(attachmentFile));
                            }
                            #endregion

                            #region store
                            using (MemoryStream stream = new MemoryStream(attachmentFile))
                            {
                                await S3.WriteObject(stream, attachmentHeader.HeaderId, attachment.ContentType);
                            }
                            #endregion

                            header.Attachment = attachmentHeader.HeaderId;
                            SQLData.InsertHeader(attachmentHeader);
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

            catch (Exception e)
            {
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<HeaderViewModel> Validate([FromBody] HashDto dto)
        {
            if (_user.IsAuthenticated) { ViewBag.User = _user.User; ViewBag.Roles = _user.Roles; }

            HeaderModel header = SQLData.GetHeaderById(dto.hash);
            
            UserProfileModel validatorProfile = SQLData.GetProfileById(_user.User);

            header.ValidatorUuid = validatorProfile.Id;
            header.ValidatorName = validatorProfile.Name;

            if (String.IsNullOrEmpty(header.ValidatorLegitimationId) || String.IsNullOrEmpty(header.IssuerUuid))
            {                
                header.IssuerUuid = validatorProfile.Id;
                header.IssuerName = validatorProfile.Name;
            }

            header.ValidationCounter = "1";

            string contractAddress = _configuration["ethContractAddress"];
            string abi = _configuration["ethAbi"];
            string senderAddress = _configuration["ethSenderAddress"];
            string senderPrimaryKey = _configuration["ethSenderPK"];

            string ethNode = _configuration["ethNode"];

            Ethereum eth = new Ethereum(contractAddress,abi,senderAddress,senderPrimaryKey,ethNode);

            header = await eth.SendToNetwork(header);

            SQLData.UpdateHeader(header);

            header = SQLData.GetHeaderWithImageById(header.HeaderId);

            return new HeaderViewModel(header, _configuration);            
        }

        [Authorize]
        [HttpPost]
        public async Task<bool> DeleteDoc(string id)
        {
            HeaderModel header = SQLData.GetHeaderById(id);

            if (header != null)
            {
                if (String.IsNullOrEmpty(header.Attachment))
                {
                    SQLData.DeleteDoc(header.Attachment);
                    S3.DeleteObject(header.Attachment);                
                }

                SQLData.DeleteDoc(id);
                S3.DeleteObject(id);
            }

            return true;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Verify(CertificateViewModel model = null)
        {
            if (_user.IsAuthenticated) { ViewBag.User = _user.User; ViewBag.Roles = _user.Roles; }
            ViewData["Title"] = "Verify";

            return View(model ?? new CertificateViewModel());
        }

        [AllowAnonymous]
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

        [Authorize]
        public IActionResult ShowDocs(HeaderListFilters filters = null)
        {
            if (_user.IsAuthenticated) { ViewBag.User = _user.User; ViewBag.Roles = _user.Roles; }

            ViewData["Title"] = "Documents";

            IEnumerable<HeaderModel> hdrList = null;            

            if (_user.HasRole("Admin"))
                hdrList = SQLData.GetHeadersWithImages();
            else
                hdrList = SQLData.GetHeadersWithImagesByIssuer(_user.User);

            if (filters != null)
            {
                if (!string.IsNullOrEmpty(filters.Id)) { hdrList = hdrList.Where(e => String.Compare(e.HeaderId, filters.Id) == 0); }
                if (!string.IsNullOrEmpty(filters.ValidatorId)) { hdrList = hdrList.Where(e => String.Compare(e.ValidatorUuid, filters.ValidatorId) == 0); }
            }

            return View(new HeaderListViewModel(hdrList, _configuration));
        }
        
        [Authorize]
        public async Task<IActionResult> EditDoc(string id)
        {
            if (_user.IsAuthenticated) { ViewBag.User = _user.User; ViewBag.Roles = _user.Roles; }

            ViewData["Title"] = "Edit Header";

            HeaderModel header = SQLData.GetHeaderById(id);

            if (!String.IsNullOrEmpty(header.ValidationCounter))
                return RedirectToAction("ShowDocs");

            return View(new HeaderViewModel(header, _configuration));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SaveDoc(HeaderViewModel model)
        {
            if (_user.IsAuthenticated) { ViewBag.User = _user.User; ViewBag.Roles = _user.Roles; }
            ViewData["Title"] = "Documents";

            if (!String.IsNullOrEmpty(model.ValidationCounter))
                return RedirectToAction("ShowDocs");

            SQLData.UpdateHeader(model.GetHeaderModel());

            return RedirectToAction("ShowDocs");
        }

        [AllowAnonymous]
        public async Task<IActionResult> Certificate(string id)
        {
            if (_user.IsAuthenticated) { ViewBag.User = _user.User; ViewBag.Roles = _user.Roles; }
            ViewData["Title"] = "Certificate";

            HeaderModel header = new HeaderModel();
            CertificateViewModel model = new CertificateViewModel();

            if (!String.IsNullOrEmpty(id))
                header = SQLData.GetHeaderWithImageById(id);

            if (header != null)            
                model = new CertificateViewModel(header, _configuration);

            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Upload(string id)
        {
            if (_user.IsAuthenticated) { ViewBag.User = _user.User; ViewBag.Roles = _user.Roles; }
            ViewData["Title"] = "Certificate";

            HeaderModel header = new HeaderModel();

            if (!String.IsNullOrEmpty(id))
                header = SQLData.GetHeaderById(id);

            CertificateViewModel model = new CertificateViewModel(header, _configuration);

            model.NotFound = TempData["NotFound"] != null;

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CertificateUpdate(CertificateViewModel model)
        {
            if (_user.IsAuthenticated) { ViewBag.User = _user.User; ViewBag.Roles = _user.Roles; }

            string id = null;

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

                    id = header?.HeaderId;                    
                }
            }

            catch (Exception e)
            {               
            }

            if (id != null)
                return RedirectToAction("Certificate", new { id = id });
            else
            {
                TempData["NotFound"] = true;
                return RedirectToAction("Upload", new { id = id });
            }
        }

        private async Task<byte[]> GetPDF(byte[] file, string id)
        {            
            byte[] ret = null;

            string baseUrl = _configuration["baseUrl"];

            string inputFilename = @"wwwroot/pdf-template/cert.html";

            string imgsrc = $"data:image/png;base64,{QR.GetPureBase64($"{baseUrl}/Contract/Certificate?id={id}")}";

            HtmlDto dto = new HtmlDto()
            {
                html = System.Web.HttpUtility.HtmlEncode(
                            Encoding.UTF8.GetString(
                                System.IO.File.ReadAllBytes(inputFilename)))
                                .Replace("{certificateid}", id)
                                .Replace("{qrlink}", imgsrc),

                footerPath = $"{baseUrl}/pdf-template/footer.html"
            };

            string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(dto);

            string uri = _configuration["pdfApi"];

            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{uri}", content).ConfigureAwait(false);

            var file2 = await response.Content.ReadAsByteArrayAsync();

            ret = Concat.DoConcat(file, file2);
            
            return ret;

        }
    }
}
