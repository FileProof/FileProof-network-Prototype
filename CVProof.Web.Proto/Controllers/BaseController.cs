using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Net.Http;

using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

using Newtonsoft.Json;

using CVProof.DAL.SQL;
using CVProof.Models;


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
                _user.Roles = String.Join(',', User.Claims.Where(e => e.Type == ClaimTypes.Role).Select(e => e.Value));
            };
        }

        protected static readonly HttpClient client = new HttpClient();
    }

    public class UserManager : IUserMgr
    {
        public string User { get; set; }

        public string Roles { get; set; }

        public bool IsAuthenticated { get; set; }

        public bool isAdmin { get { return User == "0x0100000000000000000000000000000000000000000000000000000000000000"; } }

        public bool HasRole(string role)
        {
            return Roles.Split(',').Where(e => String.Compare(e,role) == 0).Any();
        }


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
}