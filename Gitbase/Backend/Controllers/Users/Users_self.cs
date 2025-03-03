using Backend.Auth;
using Backend.Constants;
using Backend.Cryptography;
using Backend.Models;
using Backend.Truncation;
using Backend.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers {
    public partial class UsersController {
        #region CREATE
        #endregion
        #region READ
        [Authorize]
        [HttpGet(Routes.Users.GET_SELF_DATA)]
        public IActionResult GetSelfData() {
            var id = TokenReader.GetUserIdFromRequest(Request);

            var entry = db.Users.FirstOrDefault(x => x.Id == id);
            if (entry == null) {
                return NotFound(RespMsgs.Users.SENDER_NOT_FOUND);
            }

            var response = new UserResponse(entry);
            return Ok(response);
        }
        #endregion
        #region UPDATE
        [Authorize]
        [HttpPut(Routes.Users.REDACT_SELF_INFO)]
        public IActionResult RedactSelfInfo([FromBody] UserInsertionData data) {
            var subjectId = TokenReader.GetUserIdFromRequest(Request);
            var entry = db.Users.FirstOrDefault(x => x.Id == subjectId);
            if (entry == null) {
                return NotFound(RespMsgs.Users.SENDER_NOT_FOUND);
            }

            var validationResult = Validator.ValidateUser(data);
            if (validationResult != String.Empty) {
                return BadRequest(validationResult);
            }

            entry.Email   = data.Email   ;
            entry.About   = data.About   ;
            entry.Links   = data.Links   ;
            entry.Company = data.Company ;

            db.Users.Update(entry);
            db.SaveChanges();

            var response = new UserResponse(entry);
            return Ok(response);
        }
        [Authorize]
        [HttpPut(Routes.Users.RENAME_SELF)]
        public IActionResult RenameSelf([FromQuery] string newUsername) {
            var subjectId = TokenReader.GetUserIdFromRequest(Request);
            var entry = db.Users.FirstOrDefault(x => x.Id == subjectId);
            if (entry == null) {
                return NotFound(RespMsgs.Users.SENDER_NOT_FOUND);
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

        [Authorize]
        [HttpPut(Routes.Users.CHANGE_PASSWORD)]
        public IActionResult ChangeSelfPassword(
            [FromBody] ChangePasswordRequest request
        ) {
            string prevPassword = request.PreviousPassword                 ;
            string newPassword  = request.NewPassword                      ;
            int    subjectId    = TokenReader.GetUserIdFromRequest(Request);

            var entry = db.Users.FirstOrDefault(x => x.Id == subjectId);
            if (entry == null) {
                return NotFound(RespMsgs.Users.SENDER_NOT_FOUND);
            }
            if (entry.Password != Hasher.Hash(prevPassword)) {
                return BadRequest(RespMsgs.Users.PREVIOUS_PASSWORD_DOESNT_MATCH);
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

        [Authorize]
        [HttpPut(Routes.Users.CHANGE_AUTHNAME)]
        public IActionResult ChangeAuthname([FromQuery] string newAuthname) {
            int subjectId = TokenReader.GetUserIdFromRequest(Request);
            var entry = db.Users.FirstOrDefault(x => x.Id == subjectId);
            if (entry == null) {
                return NotFound(RespMsgs.Users.SENDER_NOT_FOUND);
            }
            if (entry.Authname == newAuthname) {
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
        [Authorize]
        [HttpDelete(Routes.Users.DELETE_SELF_ACCOUNT)]
        public IActionResult RemoveSelfAccount() {
            var subjectId = TokenReader.GetUserIdFromRequest(Request);
            var entry = db.Users.FirstOrDefault(x => x.Id == subjectId);
            if (entry == null) {
                return NotFound(RespMsgs.Users.SENDER_NOT_FOUND);
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
