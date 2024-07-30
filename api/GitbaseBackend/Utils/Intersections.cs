using GitbaseBackend.Data;
using GitbaseBackend.Models;
using Microsoft.AspNetCore.Mvc;

namespace GitbaseBackend.Utils {
    public class Intersections {
        public const string AUTHNAME_IS_OCCUPIED = "Authname is occupied.";
        public static bool IsNameUnique(ApplicationContext db, Repository repository) {
            var nameCheckingEntry = db.Repositories.FirstOrDefault(
                x =>
                x.Name == repository.Name &&
                x.OwnerId == repository.OwnerId
            );
            if (nameCheckingEntry != null) {
                return false;
            } else {
                return true;
            }
        }
        public static bool IsNameUnique(ApplicationContext db, IConfiguration config, User user) { 
            var nameCheckingEntry = db.Users.FirstOrDefault(x => x.Username == user.Username);
            if (nameCheckingEntry != null) {
                return false;
            } else {
                string[] exceptions = config.GetSection(Shared.EXCEPTIONS_CONFIG_KEY).Get<string[]>();
                foreach(var exception in exceptions) {
                    if (user.Username == exception) {
                        return false;
                    }
                }
                return true;
            }
        }
        public static bool IsAuthnameUnique(ApplicationContext db, User user) {
            var nameCheckingEntry = db.Users.FirstOrDefault(x => x.Authname == user.Authname);
            if (nameCheckingEntry == null) {
                return true;
            } else {
                return false;
            }
        }
        public static string GetIntersectionMessage(ApplicationContext db, IConfiguration config, User user) {
            if(!IsNameUnique(db, config, user)) {
                return Shared.NAME_IS_OCCUPIED;
            }
            if(!IsAuthnameUnique(db, user)) {
                return AUTHNAME_IS_OCCUPIED;
            }
            return String.Empty;
        }
    }
}
