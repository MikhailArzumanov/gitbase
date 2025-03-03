using Backend.Constants;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers {
    public partial class RepositoriesController {
        [Authorize(Roles=Roles.ADMIN)]
        [HttpPost(Routes.Reposes.ADMIN_CREATE_REPOS)]
        public IActionResult CreateRepository(
            [FromBody] RepositoryInsertionData data
        ) {
            var errorResponse = DataValidationPipeline(data);
            if (errorResponse != null) {
                return errorResponse;
            }

            var owner = db.Users.FirstOrDefault(x => x.Id == data.OwnerId);
            if (owner == null) {
                return NotFound(RespMsgs.Repositories.OWNER_NOT_FOUND);
            }
            var repository = new Repository {
                Name        = data.Name        ,
                Description = data.Description ,
                IsPrivate   = data.IsPrivate   ,
                OwnerId     = owner.Id         ,
            };

            db.Repositories.Add(repository);
            db.SaveChanges();

            plHandler.CreateRepository(
                repository.Name,
                owner.UserName,
                !repository.IsPrivate
            );

            var response = new RepositoryResponse(repository);
            return Ok(response);
        }

        [Authorize(Roles=Roles.ADMIN)]
        [HttpPut(Routes.Reposes.ADMIN_RENAME_REPOS)]
        public IActionResult RenameRepository(
            [FromRoute] int    id     ,
            [FromQuery] string newName
        ) {
            var repository = db.Repositories.FirstOrDefault(x => x.Id == id);
            if (repository == null) {
                return NotFound(RespMsgs.Repositories.ID_NOT_FOUND);
            }
            var owner = db.Users.FirstOrDefault(x => x.Id == repository.OwnerId);
            if (owner == null) {
                return NotFound(RespMsgs.Repositories.OWNER_NOT_FOUND);
            }

            var previousName = repository.Name;
            if(previousName == newName) {
                return BadRequest(RespMsgs.Repositories.NAMES_DOES_NOT_DIFFERS);
            }
            repository.Name = newName;

            var errorResponse = DataValidationPipeline(repository);
            if (errorResponse != null) {
                return errorResponse;
            }

            db.Repositories.Update(repository);
            db.SaveChanges();

            plHandler.RenameRepository(previousName, newName, owner.UserName);

            var response = new RepositoryResponse(repository);
            return Ok(response);
        }

        [Authorize(Roles=Roles.ADMIN)]
        [HttpDelete(Routes.Reposes.ADMIN_REMOVE_REPOS)]
        public IActionResult RemoveRepository([FromRoute] int id) {
            var repository = db.Repositories.FirstOrDefault(x => x.Id == id);
            if (repository == null) {
                return NotFound(RespMsgs.Repositories.ID_NOT_FOUND);
            }

            var owner = db.Users.FirstOrDefault(x => x.Id == repository.OwnerId);
            if (owner == null) {
                return NotFound(RespMsgs.Repositories.OWNER_NOT_FOUND);
            }

            db.Repositories.Remove(repository);
            db.SaveChanges();

            plHandler.RemoveRepository(repository.Name, owner.UserName);

            var response = new RepositoryResponse(repository);
            return Ok(response);
        }
    }
}
