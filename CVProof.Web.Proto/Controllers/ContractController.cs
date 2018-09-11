using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

using Microsoft.IdentityModel.Tokens;
using CVProof.DAL.SQL;
using CVProof.Models;


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

                header.HeaderId = Utils.Convert.ToHexString(header.GetHashBytes());

                SQLData.InsertHeader(header);
            }            
        }


        [HttpPost]
        public async Task SaveFile(ValidateViewModel model)
        {
            HeaderModel header = new HeaderModel();
            header.ValidatorUuid = model.Validator;
            header.ValidatorName = model.Validator;
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

                        header.HeaderId = Utils.Convert.ToHexString(header.GetHashBytes());

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
            header.ValidatorName = _user.User;
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

        [AllowAnonymous]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
