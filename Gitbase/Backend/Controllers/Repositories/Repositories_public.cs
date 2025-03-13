using Backend.Auth;
using Backend.Constants;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers {
    public partial class RepositoriesController {
        [Authorize]
        [HttpGet(Routes.Reposes.GET_LIST)]
        public IActionResult GetList(
            [FromQuery] uint pageNumber = 1  ,
            [FromQuery] uint pageSize   = 12
        ) {
            if (pageNumber == 0 || pageSize == 0) {
                return BadRequest(RespMsgs.Pagination.PAGINATION_IS_NOT_VALID);
            }
            var entries = db.Repositories.Where(x => !x.IsPrivate);
            var response = new RepositoriesResponse(entries, pageNumber, pageSize);
            return Ok(response);
        }

        [Authorize]
        [HttpGet(Routes.Reposes.GET_OWNED)]
        public IActionResult GetListByUser(
            [FromRoute]  int userId          ,
            [FromQuery] uint pageNumber = 1  , 
            [FromQuery] uint pageSize   = 12
        ) {
            if (pageNumber == 0 || pageSize == 0) {
                return BadRequest(RespMsgs.Pagination.PAGINATION_IS_NOT_VALID);
            }
            var senderId = TokenReader.GetUserIdFromRequest(Request);
            var entries = db.Repositories
                .Where(x => x.OwnerId == userId && (
                    !x.IsPrivate
                    || x.OwnerId == senderId
                    || x.Participators.Any(x => x.Id == senderId)
                ));
            var response = new RepositoriesResponse(entries, pageNumber, pageSize);
            return Ok(response);
        }

        [Authorize]
        [HttpGet(Routes.Reposes.GET_PARTICIPATED)]
        public IActionResult GetParticipated(
            [FromRoute]  int userId          ,
            [FromQuery] uint pageNumber = 1  ,
            [FromQuery] uint pageSize   = 12
        ) {
            if (pageNumber == 0 || pageSize == 0) {
                return BadRequest(RespMsgs.Pagination.PAGINATION_IS_NOT_VALID);
            }
            var senderId = TokenReader.GetUserIdFromRequest(Request);
            var entries = db.Repositories.Include(x => x.Participators)
                .Where(x => x.Participators.Any(x => x.Id == userId) 
                    && (!x.IsPrivate || x.OwnerId == senderId
                    || x.Participators.Any(x => x.Id == senderId)
                ));
            var response = new RepositoriesResponse(entries, pageNumber, pageSize);
            return Ok(response);
        }

        [Authorize]
        [HttpGet(Routes.Reposes.GET_CONCRETE)]
        public IActionResult GetConcrete([FromRoute] int id) {
            var senderId = TokenReader.GetUserIdFromRequest(Request);
            var entry = db.Repositories.Include(x => x.Participators)
                .FirstOrDefault(x => x.Id == id && (!x.IsPrivate
                    || x.OwnerId == senderId
                    || x.Participators.Any(x => x.Id == senderId)
                ));
            if (entry == null) {
                return NotFound(RespMsgs.Repositories.ID_NOT_FOUND);
            }

            return Ok(entry);
        }
    }
}
