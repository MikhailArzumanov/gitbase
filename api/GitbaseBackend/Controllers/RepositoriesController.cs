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
    public class RepositoriesController : ControllerBase {

        private ApplicationContext db;
        private IConfiguration config;

        private Pipelines pipelinesHandler;

        public RepositoriesController(ApplicationContext context, IConfiguration config) {
            db = context;
            this.config = config;
            pipelinesHandler = new Pipelines(config);
        }

        [HttpGet(Routes.Reposes.GET_LIST)]
        public IActionResult GetList([FromQuery] int offset, [FromQuery] int count = 100) {
            var entries = db.Repositories
                .Where(x => !x.IsPrivate)
                .OrderBy(x => x.Id)
                .Skip(offset)
                .Take(count);

            return Ok(entries);
        }

        [HttpGet(Routes.Reposes.GET_OWNED)]
        public IActionResult GetListByUser([FromRoute] int userId, [FromQuery] bool showPrivate = false) {
            var entries = db.Repositories
                .Where(x =>
                        x.OwnerId == userId
                    && (showPrivate || !x.IsPrivate)
                );

            return Ok(entries);
        }

        [HttpGet(Routes.Reposes.GET_CONCRETE)]
        public IActionResult GetConcrete([FromRoute] int id) {
            var entry = db.Repositories.Include(x => x.Collaborators).FirstOrDefault(x => x.Id == id);
            if (entry == null) {
                return NotFound(Shared.REPOSITORY_NOT_FOUND);
            }

            return Ok(entry);
        }

        [HttpPost(Routes.Reposes.CREATE_REPOS)]
        public IActionResult CreateRepository([FromBody] Repository repository) {
            if(!Validator.IsRepositoryNameValid(repository.Name)) {
                return BadRequest(Shared.REPOSITORY_NAME_IS_NOT_VALID);
            }

            if(!Intersections.IsNameUnique(db, repository)) {
                return BadRequest(Shared.NAME_IS_OCCUPIED);
            }

            var owner = db.Users.FirstOrDefault(x => x.Id == repository.OwnerId);
            if(owner == null) {
                return NotFound(Shared.OWNER_NOT_FOUND);
            }

            repository.Owner = owner;
            
            db.Repositories.Add(repository);
            db.SaveChanges();

            pipelinesHandler.CreateRepository(repository.Name, owner.Username, !repository.IsPrivate);

            return Ok(repository);
        }

        [HttpPut(Routes.Reposes.RENAME_REPOS)]
        public IActionResult RenameRepository([FromRoute] int id, [FromQuery] string newName) {
            var repository = db.Repositories.FirstOrDefault(x => x.Id == id);
            
            if(repository == null) {
                return NotFound(Shared.REPOSITORY_NOT_FOUND);
            }

            var previousName = repository.Name;

            var owner = db.Users.FirstOrDefault(x => x.Id == repository.OwnerId);
            if(owner == null) {
                return NotFound(Shared.OWNER_NOT_FOUND);
            }

            repository.Name = newName;

            if(!Intersections.IsNameUnique(db, repository)) {
                return BadRequest(Shared.NAME_IS_OCCUPIED);
            }

            db.Repositories.Update(repository);
            db.SaveChanges();

            pipelinesHandler.RenameRepository(previousName, newName, owner.Username);

            return Ok(repository);
        }

        [HttpDelete(Routes.Reposes.REMOVE_REPOS)]
        public IActionResult RemoveRepository([FromRoute] int id) { 
            var repository = db.Repositories.FirstOrDefault(x => x.Id == id);

            if(repository == null) {
                return NotFound(Shared.REPOSITORY_NOT_FOUND);
            }

            var owner = db.Users.FirstOrDefault(x => x.Id == repository.OwnerId);
            if(owner == null) {
                return NotFound(Shared.OWNER_NOT_FOUND);
            }

            db.Repositories.Remove(repository);
            db.SaveChanges();

            pipelinesHandler.RemoveRepository(repository.Name, owner.Username);

            return Ok(repository);
        }

        [HttpPut(Routes.Reposes.ADD_PARTICIPANT)]
        public IActionResult AddParticipant([FromRoute] int repId, [FromRoute] int usrId) {
            return Responses.NOT_IMPLEMENTED;
        }

        [HttpPut(Routes.Reposes.REMOVE_PARTICIPANT)]
        public IActionResult RemoveParticipant([FromRoute] int repId, [FromRoute] int usrId) {
            return Responses.NOT_IMPLEMENTED;
        }
    }
}
