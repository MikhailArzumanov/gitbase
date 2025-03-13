using Backend.Auth;
using Backend.Constants;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("CorsAllowAny")]
    public partial class AdministratorController : ControllerBase {
        private readonly TokenBuilder tokenBuilder;
        private readonly string administratorPassword;

        private void InitPassword(
            IConfiguration config,
            out string administratorPassword
        ) {
            var adminPassValue = config["Administrator:Password"];
            if (adminPassValue == null) {
                throw new Exception(ErrMsgs.ADMIN_CONFIG_IS_NOT_VALID);
            }
            administratorPassword = adminPassValue.ToString();
        }
        private void InitTokenBuilder(
            IConfiguration   config,
            out TokenBuilder tokenBuilder
        ) {
            tokenBuilder = new TokenBuilder(config);
        }
        public AdministratorController(IConfiguration config) {
            InitPassword(config, out administratorPassword);
            InitTokenBuilder(config, out tokenBuilder);
        }
        public class TokenRequestBody {
            public string Password { get; set; } = String.Empty;
        }
        public class TokenResponse {
            public string Token { get; set; } = String.Empty;
            public TokenResponse(string token) {
                Token = token;
            }
        }
    }
}
