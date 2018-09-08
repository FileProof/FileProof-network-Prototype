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
using CVProof.DAL.ETH;
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

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ValidateText(ValidateViewModel model)
        {
            HeaderModel header = new HeaderModel();            
            header.Init();
            header.ValidatorName = "User";
            header.Category = Category.Text.ToString();

            if (!String.IsNullOrEmpty(model.Text))
            {
                using (System.Security.Cryptography.HashAlgorithm sha256 = System.Security.Cryptography.SHA256.Create())
                {
                    //ret.DataHash = Encoding.UTF8.GetString(hashBytes);                    
                    header.DataHash = Utils.Convert.ToHexString(sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(model.Text)));
                }              

                Ethereum eth = new Ethereum();

                header = await eth.SendToNetwork(header);

                SQLData.SetHeader(header);
            }

            return RedirectToAction("Validate");
        }

        [HttpPost]
        public async Task<IActionResult> ValidateFile(ValidateViewModel model)
        {
            HeaderModel header = new HeaderModel();            
            header.ValidatorName = "User";
            header.Category = Category.File.ToString();

            try
            {
                foreach (var file in model.Files)
                {
                    header.Init();                    

                    if (file != null && file.Length > 0)
                    {                
                        using (var stream = file.OpenReadStream())
                        {
                            using (System.Security.Cryptography.HashAlgorithm sha256 = System.Security.Cryptography.SHA256.Create())
                            {
                                header.DataHash = Utils.Convert.ToHexString(sha256.ComputeHash(stream));
                            }
                        }

                        Ethereum eth = new Ethereum();                        

                        header = await eth.SendToNetwork(header);

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

            //TempData["header"] = JsonConvert.SerializeObject(header);

            return RedirectToAction("Validate");

        }


        public async Task<IActionResult> Verify(VerifyViewModel model = null)
        {
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

        public IActionResult ShowDocs()
        {
            return View(new HeaderListViewModel(SQLData.GetHeaders()));
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
