using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using GWeb.Models;
using System.Linq;
namespace GWeb.library
{
  
        public class Auth :IJwtAuth
        {
          
            private readonly string key="5456456sdsads";
 
        public Auth(string key)
            {
                this.key = key;

            }
            public string Authentication(string username, string password)
            {
                return  token(username);
            }
            public string token(string userName)
        {

            // 1. Create Security Token Handler
            var tokenHandler = new JwtSecurityTokenHandler();

            // 2. Create Private Key to Encrypted
            var tokenKey = Encoding.ASCII.GetBytes(key);

            //3. Create JETdescriptor
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, userName)
                    }),
                Expires = DateTime.UtcNow.AddDays(15),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            //4. Create Token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // 5. Return Token from method
            return tokenHandler.WriteToken(token);
        }

        public string WeChatAuthentication(string openId)
        {
            return token(openId);
        }
    }
    }

