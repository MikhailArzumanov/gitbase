using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

using GitbaseBackend.Models;
using Microsoft.AspNetCore.Mvc;

namespace GitbaseBackend.Utils {
    public class TokensHandler {
        private const string TOKEN_HEADER_KEY  = "Authorization";

        private const string ROLES_PAYLOAD_KEY = "roles";

        private const string JWT_KEY_CONFIG_KEY      = "Jwt:Key"      ;
        private const string JWT_LIFETIME_CONFIG_KEY = "Jwt:LifeTime" ;

        private const string SECURITY_ALGORITHM = SecurityAlgorithms.HmacSha384;

        public static string BuildToken(string[] roles, int userId, IConfiguration config) {
            
            var keyStr    = Encoding.UTF8.GetBytes(config[JWT_KEY_CONFIG_KEY]);
            var key       = new SymmetricSecurityKey(keyStr);

            var algorithm = SECURITY_ALGORITHM;

            var creds  = new SigningCredentials(key, algorithm);

            var lifetime = Double.Parse(config[JWT_LIFETIME_CONFIG_KEY]);
            var expires  = DateTime.Now.AddMinutes(lifetime);

            var token = new JwtSecurityToken(
                expires: expires,
                signingCredentials: creds
            );

            token.Payload[         ROLES_PAYLOAD_KEY] = roles;
            token.Payload[Shared.USER_ID_PAYLOAD_KEY] = userId;

            var handler  = new JwtSecurityTokenHandler();
            var tokenStr = handler.WriteToken(token);

            return tokenStr;
        }

        public static string GetTokenFromRequest(HttpRequest request) {
            var header = request.Headers[TOKEN_HEADER_KEY].ToString() 
                ?? request.Headers[TOKEN_HEADER_KEY.ToLower()].ToString();
            var token = header["Bearer ".Length..];
            return token;
        }

        public static JwtSecurityToken DecodeToken(string token) {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            return jwtToken;
        }
        public static Token GetTokenModel(string token) {
            var jwtToken = DecodeToken(token);

            var roles = (string[])jwtToken.Payload[ROLES_PAYLOAD_KEY];
            var userId = Convert.ToInt32(jwtToken.Payload[Shared.USER_ID_PAYLOAD_KEY]);

            var tokenModel = new Token{
                Self    = token  ,
                Roles   = roles  ,
                UserId  = userId ,
            };

            return tokenModel;
        }
    }
}
