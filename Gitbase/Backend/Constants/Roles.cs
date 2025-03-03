namespace Backend.Constants {
    public class Roles {
        public const string USER  = "User"         ;
        public const string ADMIN = "Administrator";
        public static readonly string[] USER_ROLES = [USER];
        public static readonly string[] ADMIN_ROLES = [ADMIN, USER];
    }
}
