using Backend.Data;
using Backend.Models;
using Backend.Cryptography;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Pipelines;
using Backend.Constants;
using Backend.Validation;
using Backend.Auth;
using Backend.Truncation;
using Microsoft.AspNetCore.Authorization;
using Backend.Models.Interfaces;
using static Backend.Controllers.RepositoriesController;

namespace Backend.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("CorsAllowAny")]
    public partial class UsersController : ControllerBase {

        private ApplicationContext db;
        private IConfiguration config;

        private UsersPipelines plHandler    ;
        private TokenBuilder   tokenBuilder ;
        private IntersectionsValidator intrsctnsValidator;

        private void InjectDependencies(
            out UsersPipelines         plHandler         ,
            out TokenBuilder           tokenBuilder      ,
            out IntersectionsValidator intrsctnsValidator
        ) {
            plHandler          = new UsersPipelines(config);
            tokenBuilder       = new TokenBuilder(config);
            intrsctnsValidator = new IntersectionsValidator(db);
        }
        public UsersController(ApplicationContext context, IConfiguration config) {
            db = context; this.config = config;
            InjectDependencies(out plHandler, out tokenBuilder, out intrsctnsValidator);
        }
        private IActionResult? EntryValidationPipeline(
            IHasAuthUserData data, int userId = 0
        ) {
            var validationMsg = Validator.ValidateUser(data)
                + Validator.ValidateAuthData(data);
            if (validationMsg != String.Empty) {
                return BadRequest(validationMsg);
            }

            var intersectionsMsg = intrsctnsValidator.Validate(data, userId);
            if(intersectionsMsg != String.Empty) {
                return BadRequest(intersectionsMsg);
            }

            return null;
        }

        #region STRUCTURES
        public class RegisterRequest : IHasAuthUserData {
            public string Authname { get; set; } = String.Empty;
            public string Password { get; set; } = String.Empty;
            public string UserName { get; set; } = String.Empty;
            public string Email    { get; set; } = String.Empty;
            public string About    { get; set; } = String.Empty;
            public string Company  { get; set; } = String.Empty;
            public string Links    { get; set; } = String.Empty;
        }
        public class ChangePasswordRequest {
            public string PreviousPassword { get; set; } = String.Empty;
            public string NewPassword      { get; set; } = String.Empty;
        }
        public class UserInsertionData : IHasUserData {
            public string UserName { get; set; } = String.Empty;
            public string Email    { get; set; } = String.Empty;
            public string About    { get; set; } = String.Empty;
            public string Company  { get; set; } = String.Empty;
            public string Links    { get; set; } = String.Empty;
        }
        public class UserResponse {
            public int    Id       { get; set; }
            public string UserName { get; set; } = String.Empty;
            public string Email    { get; set; } = String.Empty;
            public string About    { get; set; } = String.Empty;
            public string Company  { get; set; } = String.Empty;
            public string Links    { get; set; } = String.Empty;
            public UserResponse(User entry) {
                Id       = entry.Id       ;
                UserName = entry.UserName ;
                Email    = entry.Email    ;
                About    = entry.About    ;
                Company  = entry.Company  ;
                Links    = entry.Links    ;
            }
        }
        public class UsersResponse {
            public uint PageNumber { get; set; }
            public uint PagesCount { get; set; }
            public uint PageSize { get; set; }
            public ICollection<UserResponse> Entries { get; set; }
            public UsersResponse(
                IQueryable<User> entries,
                uint pageNumber,
                uint pageSize
            ) {
                var entriesCount = (decimal)entries.Count();

                PageSize = pageSize; PageNumber = pageNumber;
                PagesCount = (uint)Math.Ceiling(entriesCount / PageSize);

                uint skipCount = pageSize * (pageNumber - 1);

                Entries = entries
                    .OrderBy(x => x.Id)
                    .Skip((int)skipCount)
                    .Take((int)pageSize)
                    .Select(x => new UserResponse(x))
                    .ToList();
            }
        }
        public class TokenResponse {
            public string        Token { get; set; } = String.Empty;
            public UserResponse? User  { get; set; } = null;
            public TokenResponse(string token, User user) {
                Token = token;
                User  = new UserResponse(user);
            }
        }
        #endregion
    }
}
