using Backend.Auth;
using Backend.Constants;
using Backend.Cryptography;
using Backend.Models;
using Backend.Truncation;
using Backend.Validation;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers {
    public partial class UsersController {
        [HttpPost(Routes.Users.REGISTRATION)]
        public IActionResult Register([FromBody] RegisterRequest registerData) {
            var validationResponse = EntryValidationPipeline(registerData);
            if (validationResponse != null) {
                return validationResponse;
            }

            var plainPassword = registerData.Password;
            registerData.Password = Hasher.Hash(plainPassword);

            var entry = new User {
                Authname = registerData.Authname,
                Password = registerData.Password,
                UserName = registerData.UserName,
                Email    = registerData.Email   ,
                About    = registerData.About   ,
                Company  = registerData.Company ,
                Links    = registerData.Links   ,
            };

            db.Users.Add(entry);
            db.SaveChanges();

            plHandler.CreateUser(registerData.UserName, plainPassword);

            var response = new UserResponse(entry);
            return Ok(response);
        }

        [HttpPost(Routes.Users.AUTHORIZATION)]
        public IActionResult Authorize([FromBody] AuthData authData) {
            var validationResponse = Validator.ValidateAuthData(authData);
            if (validationResponse != String.Empty) {
                return BadRequest(validationResponse);
            }

            var hashedPassword = Hasher.Hash(authData.Password);

            var entry = db.Users.FirstOrDefault(x =>
                x.Authname == authData.Authname 
                && x.Password == hashedPassword
            );
            if (entry == null) {
                return NotFound(RespMsgs.Users.AUTH_DATA_NOT_FOUND);
            }

            var token = tokenBuilder.BuildToken(Roles.USER_ROLES, entry.Id);

            var response = new TokenResponse(token, entry);
            return Ok(response);
        }
    }
}
