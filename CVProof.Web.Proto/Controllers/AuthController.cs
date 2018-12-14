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



namespace CVProof.Web.Controllers
{
    public class AuthController : BaseController
    {
        public AuthController(IConfiguration configuration, IUserMgr user) : base(configuration, user) { }


        [HttpPost]
        public JsonResult Login([FromBody]HashDto dto)
        {
            object ret = null;
            HeaderModel header = null;
            string[] roles = null;

            if (!String.IsNullOrEmpty(dto?.hash))
            {
                header = SQLData.GetHeaderByNonce(dto.hash);                

                roles = header.SelfProfile?.Roles.Split(',') ?? new string[]{"User"};

                ret = new { id = header?.HeaderId, roles = roles };                
            }

            if (header != null)
            {
                var cookieOptions = new CookieOptions()
                {
                    Path = "/",
                    HttpOnly = false,
                    Expires = DateTimeOffset.UtcNow.AddDays(int.Parse(_configuration["JwtExpireDays"]))
                    //SameSite = SameSiteMode.None,
                    //Domain = "localhost",
                    //Secure = false
                };

                Response.Cookies.Append("authtoken",
                                        GenerateJwtToken(header.HeaderId, roles),
                                        cookieOptions);
                //else
                //throw new ApplicationException("INVALID_LOGIN_ATTEMPT");
                //TODO: implement errorhandling middleware, concerning 500s and nginx route errors as well
                ViewBag.User = header.HeaderId;
                ViewBag.Roles = header.SelfProfile.Roles;
            }

            return new JsonResult(ret);
        }

        public void Logout()
        {
            var cookieOptions = new CookieOptions()
            {
                Path = "/",
                Expires = DateTime.UtcNow.AddHours(-1)
                //HttpOnly = true,
                //Secure = false,
            };


            Response.Cookies.Append("authtoken",
                                    "",
                                    cookieOptions);
            ViewBag.User = null;
            ViewBag.Roles = null;
        }


        private string GenerateJwtToken(string id, string[] roles = null)
        {
            var claims = new List<Claim>
            {
                //        new Claim(JwtRegisteredClaimNames.Sub, "ehote@list.ru"),
                //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),                

                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, id)            
            };

            if (roles != null)
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));
            var issuer = _configuration["JwtIssuer"];

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: null,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token).ToString();
        }
    }
}
