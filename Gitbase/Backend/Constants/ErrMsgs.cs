namespace Backend.Constants {
    public class ErrMsgs {
        public const string DB_INIT_ERROR_OCCURED = 
            "При инициализации базы данных была встречена ошибка.";
        public const string TESTS_ERROR_OCCURED =
            "Error occured during one of the tests.";
        public const string JWT_CONFIG_IS_NOT_VALID =
            "Конфигурация JWT-токена невалидна";
        public const string SYSTEM_FAMILY_CONFIG_IS_NOT_VALID =
            "Данные о системе в файлах конфигурации невалидны.";
        public const string ADMIN_CONFIG_IS_NOT_VALID =
            "Данные администратора в файлах конфигурации невалидны.";
    }
}
