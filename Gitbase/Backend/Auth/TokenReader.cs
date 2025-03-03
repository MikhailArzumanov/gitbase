using Backend.Constants;
using Backend.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace Backend.Auth {
    public class TokenReader {
        private static JwtSecurityToken DecodeToken(string token) {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            return jwtToken;
        }
        private static Token? GetTokenModel(string token) {
            var jwtToken = DecodeToken(token);

            var rolesValue = jwtToken.Payload[Tokens.ROLES_PAYLOAD_KEY];
            if(rolesValue == null) {
                return null;
            }
            var rolesStr = rolesValue.ToString();
            var roles = JsonSerializer.Deserialize<string[]>(rolesStr ?? "[]") ?? [];

            var userIdData = jwtToken.Payload[Tokens.USER_ID_PAYLOAD_KEY];
            var userId = Convert.ToInt32(userIdData);

            var tokenModel = new Token {
                Self   = token  ,
                Roles  = roles  ,
                UserId = userId ,
            };
            return tokenModel;
        }
        public static Token? GetTokenFromRequest(HttpRequest request) {
            string? tokenHeader = request.Headers[Tokens.TOKEN_HEADER_KEY].ToString();
            if(tokenHeader == null) {
                return null;
            }
            var prefix = Tokens.TOKEN_HEADER_PREFIX;
            var tokenStr = tokenHeader[prefix.Length..];
            var token = GetTokenModel(tokenStr);
            return token;
        }
        public static int GetUserIdFromRequest(HttpRequest request) {
            var token = GetTokenFromRequest(request);
            if(token == null) {
                return 0;
            } else {
                return token.UserId;
            }
        }
    }
}
