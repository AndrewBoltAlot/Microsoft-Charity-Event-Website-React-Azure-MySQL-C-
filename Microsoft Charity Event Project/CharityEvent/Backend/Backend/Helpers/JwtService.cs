using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Helpers
{
    public class JwtService
    {
        private String securitykey = "ShubhamAndrewNikolaRakshandaAnuragMayXinhao";

        public String generate(String email)
        {
            var symmetricsecuritykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securitykey));
            var credentials = new SigningCredentials(symmetricsecuritykey, SecurityAlgorithms.HmacSha256Signature);
            var header = new JwtHeader(credentials);
            var payload = new JwtPayload(email,null,null,null,DateTime.Today.AddDays(1));

            var Token = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(Token);
        }

        public JwtSecurityToken validate(String jwt)
        {
            var tokenhandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(securitykey);
            tokenhandler.ValidateToken(jwt, new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false
            }, out SecurityToken validatedtoken);
            return (JwtSecurityToken)validatedtoken;
        }
    }
}
