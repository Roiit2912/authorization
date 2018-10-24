using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Newtonsoft.Json;
using System.Text;


namespace sample
{
    public class TokenManager
    {

        //HMACSHA256 hmac = new HMACSHA256();
        //string key = Convert.ToBase64String(hmac.Key);
        private static string Secret = "XCAP05H6LoKvbRRa/QkqLNMI7cOHguaRyHzyg7n5qEkGjQmtBhz4SzYh4Fqwjyi3KJHlSXKPwVu2+bXr6CtpgQ==";

        public static string GenerateToken(string Email)
        {
            byte[] key = Convert.FromBase64String(Secret);
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);

         
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, Email)}),

                Expires = DateTime.Now.AddMinutes(30),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            //token.Payload["Designation"] = "Learner";
            var response= handler.WriteToken(token);
            return JsonConvert.SerializeObject(response);
        }

        // public static string ValidateToken(string token)
        // {
        //     string username = null;
        //     ClaimsPrincipal principal = GetPrincipal(token);

        //     if (principal == null)
        //         return null;

        //     ClaimsIdentity identity = null;
        //     try
        //     {
        //         identity = (ClaimsIdentity)principal.Identity;
        //     }
        //     catch (NullReferenceException)
        //     {
        //         return null;
        //     }

        //     Claim usernameClaim = identity.FindFirst(ClaimTypes.Name);
        //     username = usernameClaim.Value;

        //     return username;
        // }

        public static ClaimsPrincipal ValidateMyToken(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);

                if (jwtToken == null)
                    return null;

                byte[] key = Convert.FromBase64String(Secret);

                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out securityToken);
                return principal;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}