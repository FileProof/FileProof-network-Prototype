using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using System.IO;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;

using CVProof.DAL.SQL;
using CVProof.Web.Models;
using CVProof.Models;
using CVProof.Utils;

namespace CVProof.Web.Controllers
{
    public class ProfileController : BaseController
    {
        public ProfileController(IConfiguration configuration, IUserMgr user) : base(configuration, user) { }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            if (_user.IsAuthenticated) { ViewBag.User = _user.User; ViewBag.Roles = _user.Roles; }
            ViewData["Title"] = "Profile";

            UserProfileModel profile = SQLData.GetProfileById(_user.User);
            HeaderModel header = SQLData.GetHeaderById(_user.User);

            var model = new UserProfileViewModel(profile);

            model.Key = header.Nonce;          

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SaveProfile(UserProfileViewModel model)
        {            
            UserProfileModel m = model.GetProfileModel();

            const double maxSize = 368640; // 350x350 24 bit bmp file size

            if (model.Files != null && model.Files.Length > 0)
            {
                byte[] imageBytes = null;

                using (MemoryStream stream = new MemoryStream())
                {
                    model.Files.CopyTo(stream);
                    imageBytes = stream.ToArray();
                }

                if (ImageHelper.isKnownFormat(imageBytes) 
                        && (ImageHelper.IsImageFit(imageBytes, 350)))
                {                    
                    m.Image = imageBytes;
                }
            }

            SQLData.UpdateProfile(m);

            return RedirectToAction("Profile");
        }


    }
}
