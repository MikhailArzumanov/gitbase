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
    public class KeysController : ControllerBase {

        private ApplicationContext db;
        private IConfiguration config;

        private Pipelines pipelinesHandler;

        const string KEY_NOT_FOUND = "Key was not found.";

        public KeysController(ApplicationContext context, IConfiguration config) {
            db = context;
            this.config = config;

            pipelinesHandler = new Pipelines(config);
        }

        [HttpGet("list/{userId}")]
        public IActionResult GetList([FromRoute] int userId) {
            var keys = db.SshKeys.Where(x => x.UserId == userId);
            return Ok(keys);
        }

        [HttpPost("add_key/{userId}")]
        public IActionResult AddKey(SshKey key, int userId) {
            var user = db.Users.Include(x => x.SshKeys).FirstOrDefault(x => x.Id == userId);
            if (user == null) {
                return NotFound(Shared.USER_NOT_FOUND);
            }

            key.UserId = user.Id;

            db.SshKeys.Add(key);
            db.SaveChanges();

            pipelinesHandler.UpdateAuthorizedKeys(user.Username, user.SshKeys);

            return Ok(key);
        }

        [HttpDelete("remove_key/{id}")]
        public IActionResult RemoveKey(int id) {
            var entry = db.SshKeys.FirstOrDefault(x => x.Id == id);
            if(entry == null) {
                return NotFound(KEY_NOT_FOUND);
            }

            db.SshKeys.Remove(entry);
            db.SaveChanges();

            var user = db.Users.Include(x => x.SshKeys).FirstOrDefault(x => x.Id == entry.UserId);
            if (user == null) {
                return new StatusCodeResult(500);
            }

            pipelinesHandler.UpdateAuthorizedKeys(user.Username, user.SshKeys);

            return Ok(entry);
        }
    }
}
