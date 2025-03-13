using Backend.Auth;
using Backend.Constants;
using Backend.Models;
using Backend.Truncation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers {
    public partial class KeysController {
        [Authorize]
        [HttpGet(Routes.Keys.GET_SELF_LIST)]
        public IActionResult GetSelfKeys(
            [FromQuery] uint pageNumber = 1,
            [FromQuery] uint pageSize   = 4
        ) {
            if (pageNumber == 0 || pageSize == 0){
                return BadRequest(RespMsgs.Pagination.PAGINATION_IS_NOT_VALID);
            }
            var subjectId = TokenReader.GetUserIdFromRequest(Request);
            var entries = db.SshKeys.Where(x => x.UserId == subjectId);
            var response = new KeysResponse(entries, pageNumber, pageSize);
            return Ok(response);
        }
        [Authorize]
        [HttpPost(Routes.Keys.ADD_SELF_KEY)]
        public IActionResult AddSelfKey([FromBody] KeyInsertionData data) {
            var subjectId = TokenReader.GetUserIdFromRequest(Request);
            var user = db.Users.FirstOrDefault(x => x.Id == subjectId);
            if (user == null) {
                return NotFound(RespMsgs.Users.ID_NOT_FOUND);
            }

            var sshKey = new SshKey {
                Name   = data.Name ,
                Self   = data.Self ,
                UserId = user.Id   ,
            };

            db.SshKeys.Add(sshKey);
            db.SaveChanges();

            plHandler.UpdateAuthorizedKeys(user.UserName, user.SshKeys);

            var response = new KeyResponse(sshKey);
            return Ok(response);
        }
        [Authorize]
        [HttpDelete(Routes.Keys.REMOVE_SELF_KEY)]
        public IActionResult RemoveSelfKey([FromRoute] int id) {
            var subjectId = TokenReader.GetUserIdFromRequest(Request);
            var user = db.Users.FirstOrDefault(x => x.Id == subjectId);
            if (user == null) {
                return NotFound(RespMsgs.Users.SENDER_NOT_FOUND);
            }

            var entry = db.SshKeys.FirstOrDefault(x => x.Id == id);
            if (entry == null) {
                return NotFound(RespMsgs.Keys.ID_NOT_FOUND);
            }
            if (entry.UserId != subjectId) {
                return BadRequest(RespMsgs.Keys.USER_IS_NOT_OWNER);
            }

            db.SshKeys.Remove(entry);
            db.SaveChanges();

            plHandler.UpdateAuthorizedKeys(user.UserName, user.SshKeys);

            var response = new KeyResponse(entry);
            return Ok(response);
        }
    }
}
