using Microsoft.IdentityModel.Tokens;

namespace Backend.Constants {
    public class Tokens {
        public const string SECURITY_ALGORITHM = 
            SecurityAlgorithms.HmacSha512;

        public const string ROLES_PAYLOAD_KEY = "roles";
        public const string USER_ID_PAYLOAD_KEY = "userId";

        public const string TOKEN_HEADER_KEY = "Authorization";
        public const string TOKEN_HEADER_PREFIX = "Bearer ";
    }
}
