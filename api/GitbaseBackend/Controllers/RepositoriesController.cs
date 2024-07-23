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

        const string REPOSITORY_NAME_IS_NOT_VALID = "Repository name is not valid.";
        const string OWNER_NOT_FOUND = "Repository owner was not found.";
        const string REPOSITORY_NOT_FOUND = "Repository was not found.";

        public RepositoriesController(ApplicationContext context, IConfiguration config) {
            db = context;
            this.config = config;
            pipelinesHandler = new Pipelines(config);
        }

        [HttpGet("list")]
        public IActionResult GetList([FromQuery] int offset, [FromQuery] int count = 100) {
            var entries = db.Repositories
                .OrderBy(x => x.Id)
                .Skip(offset)
                .Take(count);

            return Ok(entries);
        }

        [HttpGet("owned_by_user/{userId}")]
        public IActionResult GetListByUser([FromRoute] int userId, [FromQuery] bool showPrivate = false) {
            var entries = db.Repositories
                .Where(x =>
                        x.OwnerId == userId
                    && (showPrivate || !x.IsPrivate)
                );

            return Ok(entries);
        }

        [HttpGet("get/{id}")]
        public IActionResult GetConcrete([FromRoute] int id) {
            var entry = db.Repositories.Include(x => x.Collaborators).FirstOrDefault(x => x.Id == id);
            if (entry == null) {
                return NotFound(REPOSITORY_NOT_FOUND);
            }

            return Ok(entry);
        }

        [HttpPost("create")]
        public IActionResult CreateRepository([FromBody] Repository repository) {
            if(!Validator.IsRepositoryNameValid(repository.Name)) {
                return BadRequest(REPOSITORY_NAME_IS_NOT_VALID);
            }
            
            var owner = db.Users.FirstOrDefault(x => x.Id == repository.OwnerId);
            if(owner == null) {
                return NotFound(OWNER_NOT_FOUND);
            }

            repository.Owner = owner;
            
            db.Repositories.Add(repository);
            db.SaveChanges();

            pipelinesHandler.CreateRepository(repository.Name, owner.Username, true);

            return Ok(repository);
        }

        [HttpPut("rename/{id}")]
        public IActionResult RenameRepository([FromRoute] int id, [FromQuery] string newName) {
            var repository = db.Repositories.FirstOrDefault(x => x.Id == id);
            
            if(repository == null) {
                return NotFound(REPOSITORY_NOT_FOUND);
            }

            var previousName = repository.Name;

            var owner = db.Users.FirstOrDefault(x => x.Id == repository.OwnerId);
            if(owner == null) {
                return NotFound(OWNER_NOT_FOUND);
            }

            repository.Name = newName;

            db.Repositories.Update(repository);
            db.SaveChanges();

            pipelinesHandler.RenameRepository(previousName, newName, owner.Username);

            return Ok(repository);
        }

        [HttpDelete("remove/{id}")]
        public IActionResult RemoveRepository([FromRoute] int id) { 
            var repository = db.Repositories.FirstOrDefault(x => x.Id == id);

            if(repository == null) {
                return NotFound(REPOSITORY_NOT_FOUND);
            }

            var owner = db.Users.FirstOrDefault(x => x.Id == repository.OwnerId);
            if(owner == null) {
                return NotFound(OWNER_NOT_FOUND);
            }

            db.Repositories.Remove(repository);
            db.SaveChanges();

            pipelinesHandler.RemoveRepository(repository.Name, owner.Username);

            return Ok(repository);
        }
    }
}
