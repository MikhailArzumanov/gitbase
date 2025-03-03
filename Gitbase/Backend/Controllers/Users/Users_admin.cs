using Backend.Auth;
using Backend.Constants;
using Backend.Cryptography;
using Backend.Models;
using Backend.Truncation;
using Backend.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Backend.Controllers {
    public partial class UsersController {
        #region CREATE
        [Authorize(Roles=Roles.ADMIN)]
        [HttpPost(Routes.Users.ADMIN_CREATE_USER)]
        public IActionResult CreateUser([FromBody] RegisterRequest data) {
            var validationResponse = EntryValidationPipeline(data);
            if (validationResponse != null) {
                return validationResponse;
            }

            var plainPassword = data.Password;
            var passwordHash = Hasher.Hash(data.Password);
            var entry = new User {
                Authname = data.Authname ,
                Password = passwordHash  ,
                UserName = data.UserName ,
                Email    = data.Email    ,
                About    = data.About    ,
                Company  = data.Company  ,
                Links    = data.Links    ,
            };

            db.Users.Add(entry);
            db.SaveChanges();

            plHandler.CreateUser(data.UserName, plainPassword);

            var response = new UserResponse(entry);
            return Ok(response);
        }
        #endregion
        #region READ

        #endregion
        #region UPDATE
        [Authorize(Roles=Roles.ADMIN)]
        [HttpPut(Routes.Users.ADMIN_REDACT_INFO)]
        public IActionResult RedactUserInfo(
            [FromBody ] UserInsertionData data  , 
            [FromRoute] int               userId
        ) {
            var entry = db.Users.FirstOrDefault(x => x.Id == userId);
            if(entry == null) {
                return NotFound(RespMsgs.Users.ID_NOT_FOUND);
            }

            var validationResult = Validator.ValidateUser(data);
            if(validationResult != String.Empty) {
                return BadRequest(validationResult);
            }

            entry.Email   = data.Email   ;
            entry.About   = data.About   ;
            entry.Company = data.Company ;
            entry.Links   = data.Links   ;

            db.Users.Update(entry);
            db.SaveChanges();

            var response = new UserResponse(entry);
            return Ok(response);
        }
        [Authorize(Roles=Roles.ADMIN)]
        [HttpPut(Routes.Users.ADMIN_RENAME_USER)]
        public IActionResult RenameUser(
            [FromQuery] string newUsername, 
            [FromRoute] int    userId
        ) {
            var entry = db.Users.FirstOrDefault(x => x.Id == userId);
            if (entry == null) {
                return NotFound(RespMsgs.Users.ID_NOT_FOUND);
            }

            var previousUsername = entry.UserName;
            if(previousUsername == newUsername) {
                return BadRequest(RespMsgs.Users.PREVIOUS_USERNAME_IS_IDENTICAL);
            }
            entry.UserName = newUsername;
            
            var validationResponse = EntryValidationPipeline(entry, entry.Id);
            if (validationResponse != null) {
                return validationResponse;
            }

            db.Users.Update(entry);
            db.SaveChanges();

            plHandler.RenameUser(previousUsername, newUsername);

            var response = new UserResponse(entry);
            return Ok(response);
        }
        [Authorize(Roles=Roles.ADMIN)]
        [HttpPut(Routes.Users.ADMIN_CHANGE_PASSWORD)]
        public IActionResult ChangePasswordAsAdministrator(
            [FromBody ] string newPassword ,
            [FromRoute] int    userId          
        ) {
            var entry = db.Users.FirstOrDefault(x => x.Id == userId);
            if (entry == null) {
                return NotFound(RespMsgs.Users.ID_NOT_FOUND);
            }
            if (!RegularExpressions.PASSWORD_REGEXP.IsMatch(newPassword)) {
                return BadRequest(RespMsgs.Users.PASSWORD_IS_NOT_VALID);
            }
            entry.Password = Hasher.Hash(newPassword);
            db.Users.Update(entry);
            db.SaveChanges();

            plHandler.ChangeUserPassword(entry.UserName, newPassword);

            var response = new UserResponse(entry);
            return Ok(response);
        }


        [Authorize(Roles=Roles.ADMIN)]
        [HttpPut(Routes.Users.ADMIN_CHANGE_AUTHNAME)]
        public IActionResult ChangeAuthnameAsAdministrator(
            [FromQuery] string newAuthname ,
            [FromRoute] int    userId
        ) {
            var entry = db.Users.FirstOrDefault(x => x.Id == userId);
            if (entry == null) {
                return NotFound(RespMsgs.Users.ID_NOT_FOUND);
            }
            if(entry.Authname == newAuthname) {
                return BadRequest(RespMsgs.Users.PREVIOUS_AUTHNAME_IS_IDENTICAL);
            }
            entry.Authname = newAuthname;

            var validationResponse = EntryValidationPipeline(entry, entry.Id);
            if (validationResponse != null) {
                return validationResponse;
            }

            db.Users.Update(entry);
            db.SaveChanges();

            var response = new UserResponse(entry);
            return Ok(response);
        }
        #endregion
        #region DELETE
        [Authorize(Roles=Roles.ADMIN)]
        [HttpDelete(Routes.Users.ADMIN_DELETE_USER)]
        public IActionResult RemoveUser([FromRoute] int userId) {
            var entry = db.Users.FirstOrDefault(x => x.Id == userId);
            if (entry == null) {
                return NotFound(RespMsgs.Users.ID_NOT_FOUND);
            }

            db.Users.Remove(entry);
            db.SaveChanges();

            plHandler.RemoveUser(entry.UserName);

            var response = new UserResponse(entry);
            return Ok(response);
        }
        #endregion
    }
}
