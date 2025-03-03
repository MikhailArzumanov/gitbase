using Backend.Auth;
using Backend.Constants;
using Backend.Models;
using Backend.Truncation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers {
    public partial class KeysController {
        #region CREATE
        [Authorize(Roles=Roles.ADMIN)]
        [HttpPost(Routes.Keys.ADMIN_ADD_KEY)]
        public IActionResult AddKey(KeyInsertionData data, int userId) {
            var user = db.Users.Include(x => x.SshKeys)
                .FirstOrDefault(x => x.Id == userId);
            if (user == null) {
                return NotFound(RespMsgs.Users.ID_NOT_FOUND);
            }

            var sshKey = new SshKey {
                Name   = data.Name,
                Self   = data.Self,
                UserId = user.Id  ,
            };

            db.SshKeys.Add(sshKey);
            db.SaveChanges();

            plHandler.UpdateAuthorizedKeys(user.UserName, user.SshKeys);

            var response = new KeyResponse(sshKey);
            return Ok(response);
        }
        #endregion
        #region READ
        [Authorize(Roles=Roles.ADMIN)]
        [HttpGet(Routes.Keys.ADMIN_GET_LIST)]
        public IActionResult GetList(
            [FromRoute] int  userId        ,
            [FromQuery] uint pageNumber = 1,
            [FromQuery] uint pageSize   = 4
        ) {
            if(pageNumber == 0 || pageSize == 0) {
                return BadRequest(RespMsgs.Pagination.PAGINATION_IS_NOT_VALID);
            }
            var entries = db.SshKeys.Where(x => x.UserId == userId);
            var response = new KeysResponse(entries, pageNumber, pageSize);
            return Ok(response);
        }
        #endregion
        #region UPDATE
        #endregion
        #region DELETE
        [Authorize(Roles=Roles.ADMIN)]
        [HttpDelete(Routes.Keys.ADMIN_REMOVE_KEY)]
        public IActionResult RemoveKey(int id) {
            var entry = db.SshKeys.FirstOrDefault(x => x.Id == id);
            if (entry == null) {
                return NotFound(RespMsgs.Keys.ID_NOT_FOUND);
            }

            db.SshKeys.Remove(entry);
            db.SaveChanges();

            var user = db.Users
                .Include(x => x.SshKeys)
                .FirstOrDefault(x => x.Id == entry.UserId);
            if (user == null) {
                return Responses.INTERNAL_ERROR;
            }

            plHandler.UpdateAuthorizedKeys(user.UserName, user.SshKeys);

            var response = new KeyResponse(entry);
            return Ok(response);
        }
        #endregion
    }
}
