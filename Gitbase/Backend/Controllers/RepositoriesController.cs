using Backend.Data;
using Backend.Models;
using Backend.Constants;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Backend.Pipelines;
using Backend.Validation;
using Backend.Truncation;
using Microsoft.AspNetCore.Authorization;
using Backend.Auth;
using Backend.Models.Interfaces;
using static Backend.Controllers.KeysController;

namespace Backend.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("CorsAllowAny")]
    public partial class RepositoriesController : ControllerBase {

        private ApplicationContext db;
        private IConfiguration config;

        private RepositoriesPipelines  plHandler;
        private IntersectionsValidator intrsctnsValidator;

        public RepositoriesController(
            ApplicationContext context, 
            IConfiguration     config
        ) {
            db = context; this.config = config;
            plHandler          = new RepositoriesPipelines (config );
            intrsctnsValidator = new IntersectionsValidator(context);
        }

        private IActionResult? DataValidationPipeline(IHasReposData data) {
            var validationMsg = Validator.Validate(data);
            if (validationMsg != String.Empty) {
                return BadRequest(validationMsg);
            }
            var intersectionsMsg = intrsctnsValidator.Validate(data);
            if(intersectionsMsg != String.Empty) {
                return BadRequest(intersectionsMsg);
            }

            return null;
        }

        public class RepositoryInsertionData : IHasReposData {
            public string Name        { get; set; } = String.Empty;
            public string Description { get; set; } = String.Empty;
            public bool   IsPrivate   { get; set; } 
            public int    OwnerId     { get; set; }
        }

        public class RepositoryResponse {
            public int    Id          { get; set; }
            public int    OwnerId     { get; set; }
            public bool   IsPrivate   { get; set; }
            public string Name        { get; set; } = String.Empty;
            public string Description { get; set; } = String.Empty;
            public RepositoryResponse(Repository repos) {
                Id          = repos.Id          ;
                OwnerId     = repos.OwnerId     ;
                IsPrivate   = repos.IsPrivate   ;
                Name        = repos.Name        ;
                Description = repos.Description ;
            }
        }
        public class RepositoriesResponse {
            public uint PageNumber { get; set; }
            public uint PagesCount { get; set; }
            public uint PageSize   { get; set; }
            public ICollection<RepositoryResponse> Entries { get; set; }
            public RepositoriesResponse(
                IQueryable<Repository> entries,
                uint pageNumber,
                uint pageSize
            ) {
                var entriesCount = (decimal) entries.Count();

                PageSize = pageSize; PageNumber = pageNumber;
                PagesCount = (uint)Math.Ceiling(entriesCount / PageSize);

                uint skipCount = pageSize * (pageNumber - 1);

                Entries = entries
                    .OrderBy(x => x.Id)
                    .Skip((int)skipCount)
                    .Take((int)pageSize)
                    .Select(x => 
                        new RepositoryResponse(x)
                    ).ToList();
            }
        }
    }
}
