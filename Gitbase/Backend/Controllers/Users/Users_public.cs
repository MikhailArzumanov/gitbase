using Backend.Auth;
using Backend.Constants;
using Backend.Truncation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers {
    public partial class UsersController {
        [Authorize]
        [HttpGet(Routes.Users.GET_LIST)]
        public IActionResult GetList(
            [FromQuery] uint pageNumber = 1  ,
            [FromQuery] uint pageSize   = 12
        ) {
            if (pageNumber == 0 || pageSize == 0) {
                return BadRequest(RespMsgs.Pagination.PAGINATION_IS_NOT_VALID);
            }
            var entries = db.Users;
            var response = new UsersResponse(
                entries, pageNumber, pageSize
            );
            return Ok(response);
        }

        [Authorize]
        [HttpGet(Routes.Users.GET_CONCRETE)]
        public IActionResult GetConcrete([FromRoute] int targetId) {
            var rqeuesterId = TokenReader.GetUserIdFromRequest(Request);

            var entry = db.Users
                .Include(x => x.OwnedRepositories)
                .Include(x => x.ParticipationRepositories)
                .FirstOrDefault(x => x.Id == targetId);
            if (entry == null) {
                return NotFound(RespMsgs.Users.ID_NOT_FOUND);
            }

            entry.Password = String.Empty;
            entry.SshKeys = [];
            entry.OwnedRepositories = entry.OwnedRepositories.Where(
                x => !x.IsPrivate || x.Participators.Any(x => x.Id == rqeuesterId)
            ).ToList();
            entry.ParticipationRepositories = entry.ParticipationRepositories.Where(
                x => !x.IsPrivate || x.Participators.Any(x => x.Id == rqeuesterId)
            ).ToList();
            return Ok(entry);
        }
    }
}
