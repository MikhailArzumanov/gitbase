using Backend.Constants;
using Backend.Data;
using Backend.Models;
using Backend.Models.Interfaces;

namespace Backend.Validation {
    public class IntersectionsValidator {
        private ApplicationContext db;
        public IntersectionsValidator(ApplicationContext context) {
            db = context;
        }
        public string Validate(IHasReposData data) {
            var reposName    = data.Name    ;
            var reposOwnerId = data.OwnerId ;
            bool isNameIntersects = db.Repositories.Any(
                x => x.OwnerId == reposOwnerId 
                &&   x.Name    == reposName
            );
            string response = isNameIntersects ? 
                RespMsgs.Repositories.NAME_IS_NOT_UNIQUE 
                : String.Empty;
            return response;
        }
        public string Validate(IHasAuthUserData user, int userId = 0) {
            var userName = user.UserName;
            var authName = user.Authname;
            bool isNameIntersects = db.Users.Any(
                x => x.UserName == userName && x.Id != userId
            );
            if (isNameIntersects) {
                return RespMsgs.Users.NAME_IS_NOT_UNIQUE;
            }
            bool isAuthnameIntersects = db.Users.Any(
                x => x.Authname == authName && x.Id != userId
            );
            if (isAuthnameIntersects) {
                return RespMsgs.Users.AUTHNAME_IS_NOT_UNIQUE;
            }
            return String.Empty;
        }
    }
}
