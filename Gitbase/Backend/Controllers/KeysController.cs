using Backend.Data;
using Backend.Models;
using Backend.Pipelines;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("CorsAllowAny")]
    public partial class KeysController : ControllerBase {
        
        private ApplicationContext db;
        private IConfiguration config;
        private KeysPipeline plHandler;

        public KeysController(ApplicationContext context, IConfiguration config) {
            db = context; this.config = config;
            plHandler = new KeysPipeline(config);
        }

        public class KeyInsertionData {
            public string Name { get; set; } = String.Empty;
            public string Self { get; set; } = String.Empty;
        }

        public class KeyResponse {
            public int    Id     { get; set; }
            public int    UserId { get; set; }
            public string Name   { get; set; } = String.Empty;
            public string Self   { get; set; } = String.Empty;
            
            public KeyResponse(SshKey entry) {
                Id     = entry.Id     ;
                Name   = entry.Name   ;
                Self   = entry.Self   ;
                UserId = entry.UserId ;
            }
        }

        public class KeysResponse {
            public uint PageNumber { get; set; }
            public uint PagesCount { get; set; }
            public uint PageSize   { get; set; }
            public ICollection<KeyResponse> Entries { get; set; }

            public KeysResponse(
                IQueryable<SshKey> entries    ,
                uint               pageNumber , 
                uint               pageSize
            ) {
                var entriesCount = (decimal) entries.Count();

                PageSize = pageSize; PageNumber = pageNumber;
                PagesCount = (uint) Math.Ceiling(entriesCount/PageSize);
                
                uint skipCount = pageSize * (pageNumber - 1);

                Entries = entries
                    .OrderBy(x => x.Id)
                    .Skip((int) skipCount)
                    .Take((int) pageSize)
                    .Select(x => new KeyResponse(x))
                    .ToList();
            }
        }
    }
}
