using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Backend.Constants;

namespace Backend.Auth {
    public class TokenBuilder {
        private IConfiguration config;
        public TokenBuilder(IConfiguration config) {
            this.config = config;
        }

        private DateTime GetTokenExpirationDate(IConfiguration config) {
            var lifetimeStr = config[Configuration.JWT_LIFETIME_KEY];
            if (lifetimeStr == null) {
                throw new ArgumentNullException(ErrMsgs.JWT_CONFIG_IS_NOT_VALID);
            }
            var lifetime = Double.Parse(lifetimeStr);
            var expires = DateTime.Now.AddMinutes(lifetime);
            return expires;
        }

        private SigningCredentials GetTokenCredentials(IConfiguration config) {
            var keyStr = config[Configuration.JWT_KEY_CONFIGURATION_KEY];
            if (keyStr == null) {
                throw new ArgumentNullException(ErrMsgs.JWT_CONFIG_IS_NOT_VALID);
            }
            var keyBytes = Encoding.UTF8.GetBytes(keyStr);
            var key = new SymmetricSecurityKey(keyBytes);
            var algorithm = Tokens.SECURITY_ALGORITHM;
            var creds = new SigningCredentials(key, algorithm);
            return creds;
        }

        public string BuildToken(string[] roles, int userId) {
            var expires = GetTokenExpirationDate(config);
            var creds   = GetTokenCredentials   (config);
            var token = new JwtSecurityToken(
                expires: expires,
                signingCredentials: creds
            );

            token.Payload[Tokens.  ROLES_PAYLOAD_KEY] = roles;
            token.Payload[Tokens.USER_ID_PAYLOAD_KEY] = userId;

            var handler = new JwtSecurityTokenHandler();
            var tokenStr = handler.WriteToken(token);

            return tokenStr;
        }
    }
}
