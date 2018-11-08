using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

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
using Newtonsoft.Json;
using System.Net.Http;

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
using Microsoft.AspNetCore.Mvc.Filters;

namespace CVProof.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IConfiguration _configuration;

        protected readonly IUserMgr _user;

        public BaseController(IConfiguration configuration, IUserMgr user)
        {
            _configuration = configuration;
            _user = user;

            SQLData.connectionString = Microsoft.Extensions.Configuration.ConfigurationExtensions.GetConnectionString(configuration, "DefaultConnection");
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            GetCurrentUser();

            base.OnActionExecuting(context);
        }


        public void GetCurrentUser()
        {
            var token = Request?.Cookies?.FirstOrDefault(e => e.Key == "authtoken");

            ClaimsPrincipal claimsPrincipal = null;

            if (!String.IsNullOrEmpty(token?.Value))
            {
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

                var parameters = new TokenValidationParameters
                {
                    ValidIssuer = _configuration["JwtIssuer"],
                    ValidAudience = _configuration["JwtIssuer"],
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"])),
                    ClockSkew = TimeSpan.Zero
                };

                SecurityToken validatedToken;
                claimsPrincipal = handler.ValidateToken(token?.Value, parameters, out validatedToken);
            }

            if (claimsPrincipal != null)
            {
                _user.IsAuthenticated = true;
                _user.User = User.Claims.FirstOrDefault(e => e.Type == ClaimTypes.NameIdentifier).Value;
            };
        }

        protected static readonly HttpClient client = new HttpClient();
    }

    public class UserManager : IUserMgr
    {
        public string User { get; set; }

        public bool IsAuthenticated { get; set; }

        public Task<bool> LoginAsync(string hash)
        {
            throw new NotImplementedException();
        }

        public Task LogoutAsync()
        {
            throw new NotImplementedException();
        }
    }

    public class JWTInHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public JWTInHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var authenticationCookieName = "authtoken";
            var cookie = context.Request.Cookies[authenticationCookieName];
            if (cookie != null)
            {                
                context.Request.Headers.Append("Authorization", "Bearer " + cookie);
            }

            await _next.Invoke(context);
        }
    }


    public class AccessToken
    {
        public string authtoken { get; set; }
    }

}