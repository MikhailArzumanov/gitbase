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


        public UsersController(ApplicationContext context, IConfiguration config) {
            db = context;
            this.config = config;
            pipelinesHandler = new Pipelines(config);
        }

        [HttpPost(Routes.Users.AUTHORIZATION)]
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

        [HttpGet(Routes.Users.GET_LIST)]
        public IActionResult GetList([FromQuery] int offset, [FromQuery] int count = 100) {
            var entries = db.Users
                .OrderBy(x => x.Id)
                .Skip(offset)
                .Take(count);
            return Ok(entries);
        }

        [HttpGet(Routes.Users.GET_CONCRETE)]
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

        [HttpGet(Routes.Users.GET_FULL_DATA)]
        public IActionResult GetFullData([FromRoute] int id) {
            return Responses.NOT_IMPLEMENTED;
        }


        [HttpPost(Routes.Users.CREATE_USER)]
        public IActionResult CreateUser([FromBody] User user) {
            var validationResult = Validator.Validate(user);
            if(validationResult != String.Empty) {
                return BadRequest(validationResult);
            }

            var intersectionMessage = Intersections.GetIntersectionMessage(db, config, user);
            if(intersectionMessage != String.Empty) {
                return BadRequest(intersectionMessage);
            }

            var plainPassword = user.Password;

            user.Password = Hasher.Hash(user.Password);

            db.Users.Add(user);
            db.SaveChanges();

            pipelinesHandler.CreateUser(user.Username, plainPassword);

            return Ok(user);
        }

        [HttpPut(Routes.Users.REDACT_INFO)]
        public IActionResult RedactUserInfo([FromBody] User data, [FromRoute] int id) {
            var entry = db.Users.FirstOrDefault(x => x.Id == id);
            if(entry == null) {
                return NotFound(Shared.USER_NOT_FOUND);
            }

            var validationResult = Validator.Validate(data);
            if(validationResult != String.Empty) {
                return BadRequest(validationResult);
            }

            entry.Email   = data.Email   ;
            entry.About   = data.About   ;
            entry.Company = data.Company ;
            entry.Links   = data.Links   ;

            db.Users.Update(entry);
            db.SaveChanges();

            return Ok(entry);
        }

        [HttpPut(Routes.Users.CHANGE_PASSWORD)]
        public IActionResult ChangePassword([FromBody] ChangePasswordRequest request, [FromRoute] int id) {
            string prevPassword = request.PreviousPassword;
            string newPassword  = request.NewPassword     ;

            var entry = db.Users.FirstOrDefault(x => x.Id == id);
            if(entry == null) {
                return NotFound(Shared.USER_NOT_FOUND);
            }
            if(entry.Password != Hasher.Hash(prevPassword)) {
                return BadRequest(Shared.PREVIOUS_PASSWORD_DOESNT_MATCH);
            }

            entry.Password = Hasher.Hash(newPassword);
            
            db.Users.Update(entry);
            db.SaveChanges();

            pipelinesHandler.ChangeUserPassword(entry.Username, newPassword);

            return Ok(entry);
        }

        [HttpPut(Routes.Users.RENAME_USER)]
        public IActionResult RenameUser([FromQuery] string newUsername, [FromRoute] int id) {
            var entry = db.Users.FirstOrDefault(x => x.Id == id);
            if(entry == null) {
                return NotFound(Shared.USER_NOT_FOUND);
            }

            var previousUsername = entry.Username;

            entry.Username = newUsername;

            if(!Intersections.IsNameUnique(db, config, entry)) {
                return BadRequest(Shared.NAME_IS_OCCUPIED);
            }

            db.Users.Update(entry);
            db.SaveChanges();

            pipelinesHandler.RenameUser(previousUsername, newUsername);

            return Ok(entry);
        }
        [HttpPut(Routes.Users.CHANGE_AUTHNAME)]
        public IActionResult ChangeAuthname([FromQuery] string newAuthname, [FromRoute] int id) {
            var entry = db.Users.FirstOrDefault(x => x.Id == id);
            if(entry == null) {
                return NotFound(Shared.USER_NOT_FOUND);
            }

            entry.Authname = newAuthname;

            if(!Intersections.IsAuthnameUnique(db, entry)) {
                return BadRequest(Intersections.AUTHNAME_IS_OCCUPIED);
            }

            db.Users.Update(entry);
            db.SaveChanges();

            return Ok(entry);
        }

        [HttpDelete(Routes.Users.REMOVE_USER)]
        public IActionResult RemoveUser([FromRoute] int id) {
            var entry = db.Users.FirstOrDefault(x => x.Id == id);
            if(entry == null) {
                return NotFound(Shared.USER_NOT_FOUND);
            }

            db.Users.Remove(entry);
            db.SaveChanges();

            pipelinesHandler.RemoveUser(entry.Username);

            return Ok(entry);
        }

        public class ChangePasswordRequest {
            public string PreviousPassword { get; set; } = String.Empty;
            public string NewPassword      { get; set; } = String.Empty;
        }
    }
}
