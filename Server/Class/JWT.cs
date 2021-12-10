using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

    public class JWT
    {
   
        public string EncryptionJWT(string username,string email)
        {

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Email, email)
                };           

            var securityKey = "123730a1-1e99-428b-9f6d-9f3ed4021234";
            var token = new JwtSecurityToken(new JwtHeader(new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey)),
                                                                                  SecurityAlgorithms.HmacSha256)),
                                             new JwtPayload(claims));
            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenStr;

        }

        public string DecryptJWT()
        {
          
            return "";
        }


    }
