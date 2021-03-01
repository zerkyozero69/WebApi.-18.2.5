using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace WebApi.Jwt
{
    public static class JwtManager
    {
        /// <summary>
        /// Use the below code to generate symmetric Secret Key
        ///     var hmac = new HMACSHA256();
        ///     var key = Convert.ToBase64String(hmac.Key);
        /// </summary>
        private const string Secret = "db3OIsj+BXE9NZDy0t8W3TcNekrF+2d/1sFnWG4HnV8TZY30iTOdtVWJG8abWvB1GlOgJuQZdcF2Luqm/hccMw==";

        public static string GenerateToken(string username,  int expireMinutes = 60)
        {
            var symmetricKey = Convert.FromBase64String(Secret);
            var tokenHandler = new JwtSecurityTokenHandler();
         //   var refresh_Token = Base64FormattingOptions(tokenHandler + ":"+expires+":"+si);

            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.Name, username),
                          new    Claim(ClaimTypes.Authentication,username)
                        }),

                Expires = now.AddMinutes(Convert.ToInt32(expireMinutes)),
               
                
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };
        
            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);
           
            return token;
        }
        //public static string GetTokenFromDB(string username, string password)
        //{
        //    UserTokenMaster userMaster = context.UserTokenMasters.FirstOrDefault(user =>
        //    user.UserName.Equals(username, StringComparison.OrdinalIgnoreCase)
        //    && user.UserPassword == password);
        //    Token token = null;
        //    if (userMaster != null)
        //    {
        //        token = new Token()
        //        {
        //            AccessToken = userMaster.AccessToken,
        //            RefreshToken = userMaster.RefreshToken,
        //            ExpiredDateTime = (DateTime)userMaster.TokenExpiredTime
        //        };
        //    }
        //    return token;
        //}

        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                    return null;

                var symmetricKey = Convert.FromBase64String(Secret);

                var validationParameters = new TokenValidationParameters()
                {
                   RequireExpirationTime = true,
                   ValidateIssuer = false,
                   ValidateAudience = false,
                   IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
                };

                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                return principal;
            }

            catch (Exception)
            {
                return null;
            }
        }
    }
}