using Backend.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers {
    public partial class AdministratorController {
        
        [HttpPost(Routes.Administrator.GET_TOKEN)]
        public IActionResult GetAdminToken([FromBody] TokenRequestBody requestBody) {
            if (requestBody.Password != administratorPassword) {
                return BadRequest(RespMsgs.Administrator.PASSWORD_IS_NOT_CORRECT);
            }
            var token = tokenBuilder.BuildToken(Roles.ADMIN_ROLES, userId: 0);
            var response = new TokenResponse(token);
            return Ok(response);
        }
    }
}
