namespace Backend.Constants {
    public class RespMsgs {
        public class Pagination {
            public const string PAGINATION_IS_NOT_VALID =
                "Параметры страниц невалидны. Значения должны быть ненулевыми.";
        }
        public class Administrator {
            public const string PASSWORD_IS_NOT_CORRECT =
                "Переаднный пароль администратора некорректен.";
        }
        public class Authentication {
            public const string SENDER_HAVE_NOT_ENOUGH_RIGHTS =
                "Отправитель запроса не обладает достаточными правами на выполнение операции.";
        }
        public class Repositories {
            public const string ID_NOT_FOUND =
                "Репозиторий с заданным идентификатором не найден.";
            public const string OWNER_NOT_FOUND =
                "Пользователь, заданный владельцем репозитория, не найден.";
            public const string USER_IS_NOT_OWNER =
                "Пользователь не является владельцем репозитория.";

            public const string NAME_IS_NOT_UNIQUE =
                "Репозиторий с заданным именем уже существует " +
                "среди репозиториев пользователя.";
            public const string NAMES_DOES_NOT_DIFFERS =
                "Новое имя репозитория совпадает с предыдущим.";

            public const string NAME_IS_NOT_VALID =
                "Имя репозитория невалидно. " +
                "Имя может содержать буквы латиницы и кириллицы, " +
                "цифры, подчеркивание и дефис.";
            public const string DESCRIPTION_IS_NOT_VALID =
                "Описание репозитория невалидно. " +
                "Описание может содержать буквы латиницы и кириллицы, " +
                "а также специальные символы стандартной клавиатуры.";
        }
        public class Users {
            public const string SENDER_NOT_FOUND =
                "Запись пользователя-отправителя не найдена.";
            public const string ID_NOT_FOUND =
                "Пользователь с заданным идентификатором не найден.";
            public const string AUTH_DATA_NOT_FOUND =
                "Пользователь с заданными авторизационными данными не найден.";
            public const string PREVIOUS_PASSWORD_DOESNT_MATCH =
                "Предыдущий пароль пользователя не совпадает.";

            public const string PREVIOUS_USERNAME_IS_IDENTICAL =
                "Предыдущее имя совпадает с задаваемым.";
            public const string PREVIOUS_AUTHNAME_IS_IDENTICAL =
                "Предыдущее авторизационное имя совпадает с задаваемым.";

            public const string NAME_IS_NOT_UNIQUE =
                "Переданное имя пользователя не является уникальным.";
            public const string AUTHNAME_IS_NOT_UNIQUE =
                "Переданное авторизационное имя пользователя не является уникальным.";

            public const string USER_NAME_IS_NOT_VALID =
                "Имя пользователя не является валидным.";
            public const string AUTHNAME_IS_NOT_VALID =
                "Авторизационное имя пользователя не является валидным.";
            public const string PASSWORD_IS_NOT_VALID =
                "Пароль пользователя не является валидным.";
            public const string EMAIL_IS_NOT_VALID =
                "Адрес электронной почты пользователя не является валидным.";
            public const string ABOUT_FIELD_IS_NOT_VALID =
                "Значение раздела 'О пользователе' не является валидным.";
        }

        public class Keys {
            public const string ID_NOT_FOUND =
                "Ключ с заданным идентификатором не найден.";
            public const string USER_IS_NOT_OWNER =
                "Пользователь не является владельцем ключа.";
        }
    }
}
