using Backend.Constants;
using Backend.Models;
using Backend.Models.Interfaces;

namespace Backend.Validation {
    public class Validator {
        public static string Validate(IHasReposData repository) {
            var nameRegexp = RegularExpressions.NAME_REGEXP;
            if (!nameRegexp.IsMatch(repository.Name)) {
                return RespMsgs.Repositories.NAME_IS_NOT_VALID;
            }
            var descriptionRegexp = RegularExpressions.DESCRIPTION_REGEXP;
            if (!descriptionRegexp.IsMatch(repository.Description)) {
                return RespMsgs.Repositories.DESCRIPTION_IS_NOT_VALID;
            }
            return String.Empty;
        }
        public static string ValidateUser(IHasUserData user) {
            var nameRegexp = RegularExpressions.USER_NAME_REGEXP;
            if (!nameRegexp.IsMatch(user.UserName)) {
                return RespMsgs.Users.USER_NAME_IS_NOT_VALID;
            }
            var emailRegexp = RegularExpressions.EMAIL_REGEXP;
            if (!emailRegexp.IsMatch(user.Email)) {
                return RespMsgs.Users.EMAIL_IS_NOT_VALID;
            }
            var aboutRegexp = RegularExpressions.DESCRIPTION_REGEXP;
            if (!aboutRegexp.IsMatch(user.About)) {
                return RespMsgs.Users.ABOUT_FIELD_IS_NOT_VALID;
            }
            return String.Empty;
        }

        public static string ValidateAuthData(IHasAuthData authData) {
            var authNameRegexp = RegularExpressions.AUTHNAME_REGEXP;
            if (!authNameRegexp.IsMatch(authData.Authname)) {
                return RespMsgs.Users.AUTHNAME_IS_NOT_VALID;
            }
            var passwordRegexp = RegularExpressions.PASSWORD_REGEXP;
            if (!passwordRegexp.IsMatch(authData.Password)) {
                return RespMsgs.Users.PASSWORD_IS_NOT_VALID;
            }
            return String.Empty;
        }
    }
}
