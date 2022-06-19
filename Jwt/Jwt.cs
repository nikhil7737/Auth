using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Models;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Jwt
{
    public class JwtHandler
    {
        private const string privateKey = "lkfjsdlfjsdklfjlsd";
        public string GetToken(User user)
        {
            var claims = new List<Claim> {
                // new Claim (JwtRegisteredClaimNames.Email, user.Email),
                new Claim (JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                new Claim ("gender1", user.Gender),
                new Claim ("role1", user.Role),
                new Claim ("age", user.Age.ToString())
            };
            var privateKeyObj = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKey));
            var signingCredentials = new SigningCredentials(privateKeyObj, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "NikhilFromJwtClass",
                audience: "toTheOneIssued",
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: signingCredentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}