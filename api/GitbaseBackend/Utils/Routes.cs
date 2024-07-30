namespace GitbaseBackend.Utils {
    public class Routes {
        public class Users {
            public const string AUTHORIZATION = "auth";

            public const string GET_LIST        = "list"                 ;
            public const string GET_CONCRETE    = "get/{id}"             ;
            public const string GET_FULL_DATA   = "get_full_data/{id}"   ;

            public const string CREATE_USER     = "create"               ;

            public const string REDACT_INFO     = "redact_info/{id}"     ;
            public const string CHANGE_PASSWORD = "change_password/{id}" ;
            public const string RENAME_USER     = "rename/{id}"          ;
            public const string CHANGE_AUTHNAME = "change_authname/{id}" ;

            public const string REMOVE_USER     = "remove/{id}"          ;
        }

        public class Reposes {
            public const string GET_LIST     = "list"                   ;
            public const string GET_OWNED    = "owned_by_user/{userId}" ;
            public const string GET_CONCRETE = "get/{id}"               ;
            public const string CREATE_REPOS = "create"                 ;
            public const string RENAME_REPOS = "rename/{id}"            ;
            public const string REMOVE_REPOS = "remove/{id}"            ;

            public const string    ADD_PARTICIPANT =    "add_participant/{repId}/{usrId}";
            public const string REMOVE_PARTICIPANT = "remove_participant/{repId}/{usrId}";
        }

        public class Keys {
            public const string GET_LIST   = "list/{userId}"    ;
            public const string ADD_KEY    = "add_key/{userId}" ;
            public const string REMOVE_KEY = "remove_key/{id}"  ;
        }
    }
}
