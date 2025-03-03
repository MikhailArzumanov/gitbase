using Backend.Auth;
using Backend.Constants;
using Backend.Models;
using Backend.Truncation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Backend.Controllers {
    public partial class RepositoriesController {
        private void SetNulls(out User usr, out Repository repos) {
            usr = new User(); repos = new Repository();
        }
        private IActionResult? OwnedEntryPipeline(
            int senderId, int reposId,
            out User sender, out Repository repository
        ) {
            var sndr = db.Users.FirstOrDefault(x => x.Id == senderId);
            if (sndr == null) {
                SetNulls(out sender, out repository);
                return NotFound(RespMsgs.Users.SENDER_NOT_FOUND);
            }
            var repos = db.Repositories.FirstOrDefault(x => x.Id == reposId);
            if (repos == null) {
                SetNulls(out sender, out repository);
                return NotFound(RespMsgs.Repositories.ID_NOT_FOUND);
            }
            if (repos.OwnerId != senderId) {
                SetNulls(out sender, out repository);
                return BadRequest(RespMsgs.Repositories.USER_IS_NOT_OWNER);
            }
            sender = sndr; repository = repos;
            return null;
        }

        [Authorize]
        [HttpPost(Routes.Reposes.CREATE_SELF_REPOS)]
        public IActionResult CreateSelfRepository(
            [FromBody] RepositoryInsertionData data
        ) {
            var senderId = TokenReader.GetUserIdFromRequest(Request);
            var sender = db.Users.FirstOrDefault(x => x.Id == senderId);
            if (sender == null) {
                return NotFound(RespMsgs.Users.SENDER_NOT_FOUND);
            }
            var errorResponse = DataValidationPipeline(data);
            if (errorResponse != null) {
                return errorResponse;
            }
            var repository = new Repository {
                Name        = data.Name        ,
                Description = data.Description ,
                IsPrivate   = data.IsPrivate   ,
                OwnerId     = sender.Id        ,
            };

            db.Repositories.Add(repository);
            db.SaveChanges();

            plHandler.CreateRepository(
                repository.Name,
                sender.UserName,
                !repository.IsPrivate
            );

            var response = new RepositoryResponse(repository);
            return Ok(response);
        }

        [Authorize]
        [HttpPut(Routes.Reposes.RENAME_SELF_REPOS)]
        public IActionResult RenameSelfRepository(
            [FromRoute] int    repId   ,
            [FromQuery] string newName
        ) {
            var senderId = TokenReader.GetUserIdFromRequest(Request);
            var errorResponse = OwnedEntryPipeline(senderId, reposId: repId,
                out User sender, out Repository repository
            );
            if (errorResponse != null) {
                return errorResponse;
            }

            var previousName = repository.Name;
            if (previousName == newName) {
                return BadRequest(RespMsgs.Repositories.NAMES_DOES_NOT_DIFFERS);
            }
            repository.Name = newName;

            var validationResponse = DataValidationPipeline(repository);
            if (validationResponse != null) {
                return validationResponse;
            }

            db.Repositories.Update(repository);
            db.SaveChanges();

            plHandler.RenameRepository(previousName, newName, sender.UserName);

            var response = new RepositoryResponse(repository);
            return Ok(response);
        }

        [Authorize]
        [HttpDelete(Routes.Reposes.REMOVE_SELF_REPOS)]
        public IActionResult RemoveSelfRepository([FromRoute] int repId) {
            var senderId = TokenReader.GetUserIdFromRequest(Request);
            var errorResponse = OwnedEntryPipeline(senderId, reposId: repId, 
                out User sender, out Repository repository
            );
            if(errorResponse != null) {
                return errorResponse;
            }

            db.Repositories.Remove(repository);
            db.SaveChanges();

            plHandler.RemoveRepository(repository.Name, sender.UserName);

            var response = new RepositoryResponse(repository);
            return Ok(response);
        }

        [Authorize]
        [HttpPut(Routes.Reposes.ADD_PARTICIPANT)]
        public IActionResult AddParticipant([FromRoute] int repId) {
            return Responses.NOT_IMPLEMENTED;
        }
        [Authorize]
        [HttpPut(Routes.Reposes.REMOVE_PARTICIPANT)]
        public IActionResult RemoveParticipant([FromRoute] int repId) {
            return Responses.NOT_IMPLEMENTED;
        }
    }
}
