namespace GitbaseBackend.Utils {
    public class Shared {

        public const string EXCEPTIONS_CONFIG_KEY = "UsernameExceptions";

        public const string USER_ID_PAYLOAD_KEY = "user_id";

        public static readonly string[] USER_ROLES = new string[] { "User" };


        public const string NAME_IS_OCCUPIED = "Name is occupied.";

        public const string USER_NOT_FOUND                 = "User was not found."                ;
        public const string PREVIOUS_PASSWORD_DOESNT_MATCH = "Previous password doesn't matches." ;

        public const string REPOSITORY_NAME_IS_NOT_VALID = "Repository name is not valid."   ;
        public const string OWNER_NOT_FOUND              = "Repository owner was not found." ;
        public const string REPOSITORY_NOT_FOUND         = "Repository was not found.";

        public const string KEY_NOT_FOUND = "Key was not found.";

    }
}
