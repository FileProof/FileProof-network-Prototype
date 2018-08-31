using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CVProof.Web.Models;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;
using CVProof.Utils;
using CVProof.DAL.SQL;
using Newtonsoft.Json;
using CVProof.Models;

namespace CVProof.Web.Controllers
{    
    public class ContractController : Controller
    {
        private readonly IConfiguration configuration;

        public ContractController(IConfiguration config)
        {
            configuration = config;
            SQLData.connectionString = Microsoft.Extensions.Configuration.ConfigurationExtensions.GetConnectionString(configuration, "DefaultConnection");
        }

        public IActionResult Validate()
        {
            ValidateViewModel model = new ValidateViewModel();

            //object header;

            //if (TempData.TryGetValue("header", out header) != null)
            //{ 
            //    model.Header = header == null ? null: JsonConvert.DeserializeObject<HeaderModel>((string) header);
            //}

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ValidateText(ValidateViewModel model)
        {
            byte[] datahash;

            HeaderModel header = new HeaderModel();

            if (!String.IsNullOrEmpty(model.Text))
            {
                using (System.Security.Cryptography.HashAlgorithm sha256 = System.Security.Cryptography.SHA256.Create())
                {
                    datahash = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(model.Text));
                }

                header.DataHash = System.Convert.ToBase64String(datahash);

                SQLData.SetHeader(header);
            }

            TempData["header"] = JsonConvert.SerializeObject(header);

            return RedirectToAction("Validate");
        }

        [HttpPost]
        public async Task<IActionResult> ValidateFile(ValidateViewModel model)
        {
            byte[] datahash;

            HeaderModel header = new HeaderModel();

            try
            {
                foreach (var file in model.Files)
                {
                    datahash = null;

                    if (file != null && file.Length > 0)
                    {
                        byte[] bytes = new byte[(int)file.Length];                            

                        using (var stream = file.OpenReadStream())
                        {
                            file.CopyToAsync(stream);

                            //await stream.ReadAsync(bytes);
                            //string base64string = System.Convert.ToBase64String(bytes);

                            using (System.Security.Cryptography.HashAlgorithm sha256 = System.Security.Cryptography.SHA256.Create())
                            {
                                datahash = sha256.ComputeHash(stream);
                            }
                        }

                        header.DataHash = System.Convert.ToBase64String(datahash);

                        SQLData.SetHeader(header);

                    }
                }
            }

            catch (FileNotFoundException e)
            {
                return Content("file not selected");
            }

            catch (IOException e)
            {
                return Content("error reading file");
            }

            TempData["header"] = JsonConvert.SerializeObject(header);

            return RedirectToAction("Validate");

        }


        //public IActionResult Verify()
        //{
        //    VerifyViewModel model = new VerifyViewModel();

        //    return View(model);
        //}

        public async Task<IActionResult> Verify(VerifyViewModel model = null)
        {
            return View(model ?? new VerifyViewModel());
        }

        [HttpPost]
        public async Task<bool> VerifyText(VerifyViewModel model)
        {
            byte[] datahash;

            HeaderModel header = new HeaderModel();

            //if (model.SubmitText != null)
            //{
                if (!String.IsNullOrEmpty(model.Text))
                {
                    using (System.Security.Cryptography.HashAlgorithm sha256 = System.Security.Cryptography.SHA256.Create())
                    {
                        datahash = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(model.Text));
                    }
                    
                    header.DataHash = System.Convert.ToBase64String(datahash);

                    model.Status = header.Equals(SQLData.GetHeaderByData(header.DataHash)) ? VerificationStatus.True : VerificationStatus.False;
                    // Temp function use, after implementing smart contract interface needs to be changed to GetById
                }
            //}

            return model.Status == VerificationStatus.True;
        }
        [HttpPost]
        public async Task<bool> VerifyFile(VerifyViewModel model)
        {
            byte[] datahash;

            HeaderModel header = new HeaderModel();
            try
            {
                foreach (var file in model.Files)
                {
                    datahash = null;

                    if (file != null && file.Length > 0)
                    {
                        byte[] bytes = new byte[(int)file.Length];

                        using (var stream = file.OpenReadStream())
                        {
                            file.CopyToAsync(stream);

                            using (System.Security.Cryptography.HashAlgorithm sha256 = System.Security.Cryptography.SHA256.Create())
                            {
                                datahash = sha256.ComputeHash(stream);
                            }
                        }

                        header.DataHash = System.Convert.ToBase64String(datahash);

                        model.Status = header.Equals(SQLData.GetHeaderByData(header.DataHash)) ? VerificationStatus.True : VerificationStatus.False;

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

        public async Task<IActionResult> Mainnet()
        {
            ViewBag.EthereumFoundationBalance = await Web3Util.GetEthereumFoundationBalance();
            return View();
        }

        public async Task<bool> RunTest(string hash)
        {           
            return await Web3Util.TestHashStore(hash);
        }

        public IActionResult ShowDocs()
        {
            List<HeaderModel> model = SQLData.GetHeaders();

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
