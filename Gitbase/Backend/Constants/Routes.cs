namespace Backend.Constants {
    public class Routes {
        public class Administrator {
            public const string GET_TOKEN = "get_admin_token";
        }
        public class Users {

            public const string ADMIN_CREATE_USER     = "admin/create"                 ;
            public const string ADMIN_CHANGE_AUTHNAME = "admin/chng_authname/{userId}" ;
            public const string ADMIN_CHANGE_PASSWORD = "admin/chng_password/{userId}" ;
            public const string ADMIN_REDACT_INFO     = "admin/redact_info/{userId}"   ;
            public const string ADMIN_RENAME_USER     = "admin/rename/{userId}"        ;
            public const string ADMIN_DELETE_USER     = "admin/delete/{userId}"        ;

            public const string REGISTRATION  = "auth/register";
            public const string AUTHORIZATION = "auth/authorize";

            public const string RENAME_SELF         = "self/rename"        ;
            public const string GET_SELF_DATA       = "self/get_data"      ;
            public const string REDACT_SELF_INFO    = "self/redact_info"   ;
            public const string CHANGE_AUTHNAME     = "self/chng_authname" ;
            public const string CHANGE_PASSWORD     = "self/chng_password" ;
            public const string DELETE_SELF_ACCOUNT = "self/delete_account";

            public const string GET_LIST        = "public/get_list"       ;
            public const string GET_CONCRETE    = "public/get/{targetId}" ;


        }

        public class Reposes {
            public const string ADMIN_CREATE_REPOS = "admin/create"     ;
            public const string ADMIN_RENAME_REPOS = "admin/rename/{id}";
            public const string ADMIN_REMOVE_REPOS = "admin/remove/{id}";

            public const string GET_LIST         = "public/get_list"           ;
            public const string GET_OWNED        = "public/owned_by/{userId}"  ;
            public const string GET_PARTICIPATED = "public/parted_by/{userId}" ;
            public const string GET_CONCRETE     = "public/get/{id}"           ;

            public const string CREATE_SELF_REPOS  = "self/create"              ;
            public const string RENAME_SELF_REPOS  = "self/rename/{repId}"      ;
            public const string REMOVE_SELF_REPOS  = "self/remove/{repId}"      ;
            public const string    ADD_PARTICIPANT = "self/add_prtcpnt/{repId}" ;
            public const string REMOVE_PARTICIPANT = "self/rmv_prtcpnt/{repId}" ;
        }

        public class Keys {
            public const string ADMIN_GET_LIST   = "admin/list/{userId}"    ;
            public const string ADMIN_ADD_KEY    = "admin/add_key/{userId}" ;
            public const string ADMIN_REMOVE_KEY = "admin/remove_key/{id}"  ;

            public const string GET_SELF_LIST   = "self/get_list"    ;
            public const string ADD_SELF_KEY    = "self/add"         ;
            public const string REMOVE_SELF_KEY = "self/remove/{id}" ;
        }
    }
}
