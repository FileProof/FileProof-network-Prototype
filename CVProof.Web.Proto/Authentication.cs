//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using CVProof.Models;
//using CVProof.DAL.SQL;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.Extensions.Configuration;

//using System.ComponentModel.DataAnnotations;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using System.Linq;
//using Microsoft.AspNetCore.Cors;
//using Microsoft.AspNetCore.Http;

//using Microsoft.IdentityModel.Tokens;

//namespace CVProof.Web
//{
//    public class Authentication
//    {
//        public bool Auth(string hash)
//        {           
            

//            if (header != null)
//            {
//                Microsoft.IdentityModel.Tokens.
//            }



//        }

//        private async Task<string> GenerateJwtToken(string hash)
//        {
//            var claims = new List<Claim>
//            {
//                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
//            };

//            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
//            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
//            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

//            var token = new JwtSecurityToken(
//                _configuration["JwtIssuer"],
//                _configuration["JwtIssuer"],
//                claims,
//                expires: expires,
//                signingCredentials: creds
//            );

//            return new JwtSecurityTokenHandler().WriteToken(token).ToString();
//        }

//    }
//}
