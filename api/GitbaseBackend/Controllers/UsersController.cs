using GitbaseBackend.Data;
using GitbaseBackend.Models;
using GitbaseBackend.Utils;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GitbaseBackend.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("CorsAllowAny")]
    public class UsersController : ControllerBase {

        private ApplicationContext db;
        private IConfiguration config;

        private Pipelines pipelinesHandler;

        const string PREVIOUS_PASSWORD_DOESNT_MATCHES = "Previous password doesn't matches.";

        public UsersController(ApplicationContext context, IConfiguration config) {
            db = context;
            this.config = config;
            pipelinesHandler = new Pipelines(config);
        }

        [HttpPost("auth")]
        public IActionResult Login([FromBody] AuthData authData) {
            var validationResponse = Validator.Validate(authData);
            if(validationResponse != String.Empty) {
                return BadRequest(validationResponse);
            }

            var hashedPassword = Hasher.Hash(authData.Password);

            var entry = db.Users.FirstOrDefault(x => 
                   x.Authname == authData.Authname 
                && x.Password == hashedPassword
            );
            if (entry == null) {
                return NotFound(Shared.USER_NOT_FOUND);
            }

            var token = TokensHandler.BuildToken(Shared.USER_ROLES, entry.Id, config);
            return Ok(new { Self = token});
        }

        [HttpGet("list")]
        public IActionResult GetList([FromQuery] int offset, [FromQuery] int count) {
            var entries = db.Users.Skip(offset).Take(count);
            return Ok(entries);
        }

        [HttpGet("get/{id}")]
        public IActionResult GetConcrete([FromRoute] int id) {
            var entry = db.Users
                .Include(x => x.OwnedRepositories)
                .Include(x => x.CollaboratedRepositories)
                .FirstOrDefault(x => x.Id == id);
            if (entry == null) {
                return NotFound(Shared.USER_NOT_FOUND);
            }

            return Ok(entry);
        }


        [HttpPost("create")]
        public IActionResult CreateUser([FromBody] User user) {
            var validationResult = Validator.Validate(user);
            if(validationResult != String.Empty) {
                return BadRequest(validationResult);
            }

            var plainPassword = user.Password;

            user.Password = Hasher.Hash(user.Password);

            db.Users.Add(user);
            db.SaveChanges();

            pipelinesHandler.CreateUser(user.Username, plainPassword);

            return Ok(user);
        }

        [HttpPut("redact_info/{id}")]
        public IActionResult RedactUserInfo([FromBody] User data, [FromRoute] int id) {
            var entry = db.Users.FirstOrDefault(x => x.Id == id);
            if(entry == null) {
                return NotFound(Shared.USER_NOT_FOUND);
            }

            entry.Email   = data.Email   ;
            entry.About   = data.About   ;
            entry.Company = data.Company ;
            entry.Links   = data.Links   ;

            db.Users.Update(entry);
            db.SaveChanges();

            return Ok(entry);
        }

        [HttpPut("change_password/{id}")]
        public IActionResult ChangePassword([FromBody] ChangePasswordRequest request, [FromRoute] int id) {
            string prevPassword = request.PreviousPassword;
            string newPassword  = request.NewPassword     ;

            var entry = db.Users.FirstOrDefault(x => x.Id == id);
            if(entry == null) {
                return NotFound(Shared.USER_NOT_FOUND);
            }
            if(entry.Password != Hasher.Hash(prevPassword)) {
                return BadRequest(PREVIOUS_PASSWORD_DOESNT_MATCHES);
            }

            entry.Password = Hasher.Hash(newPassword);
            
            db.Users.Update(entry);
            db.SaveChanges();

            pipelinesHandler.ChangeUserPassword(entry.Username, newPassword);

            return Ok(entry);
        }

        [HttpPut("rename/{id}")]
        public IActionResult RenameUser([FromQuery] string newUsername, [FromRoute] int id) {
            var entry = db.Users.FirstOrDefault(x => x.Id == id);
            if(entry == null) {
                return NotFound(Shared.USER_NOT_FOUND);
            }

            var previousUsername = entry.Username;

            entry.Username = newUsername;

            db.Users.Update(entry);
            db.SaveChanges();

            pipelinesHandler.RenameUser(previousUsername, newUsername);

            return Ok(entry);
        }

        [HttpDelete("remove/{id}")]
        public IActionResult RemoveUser([FromRoute] int id) {
            var entry = db.Users.FirstOrDefault(x => x.Id == id);
            if(entry == null) {
                return NotFound(Shared.USER_NOT_FOUND);
            }

            db.Users.Remove(entry);
            db.SaveChanges();

            return Ok(entry);
        }

        public class ChangePasswordRequest {
            public string PreviousPassword { get; set; } = String.Empty;
            public string NewPassword      { get; set; } = String.Empty;
        }
    }
}
