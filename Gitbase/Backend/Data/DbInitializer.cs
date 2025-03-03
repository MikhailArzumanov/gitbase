namespace Backend.Data {
    #region Класс DbInitializer
    /** Класс DbInitializer
     * <summary>
     *  Включает в себя логику инициализации базы данных.
     * </summary>
     */
    public class DbInitializer {
        #region Поля.
        /** Поле db.
         * <summary>
         *  Поле ORM-интерфейса базы данных.
         * </summary>
         */
        private ApplicationContext db;
        #endregion
        #region Конструктор.
        /** Конструктор DbInitializer(ApplicationContext)
         * <summary>
         *  Данный конструктор инъектирует объект контекста в инциализатор.
         * </summary>
         * <param name="db">
         *  Объект контекста.
         * </param>
         */
        private DbInitializer(ApplicationContext db) {
            this.db = db;
        }
        #endregion
        #region Функционал частной инициализации.
        /** Функция PrefillUsersTable
         * <summary>
         *  Функция осуществляет заполнение таблицы пользователей.
         * </summary>
         */
        private void PrefillUsersTable() {

        }
        /** Функция PrefillMainTables
         * <summary>
         *  Функция осуществляет заполнение основных таблиц БД.
         * </summary>
         */
        private void PrefillMainTables() {
            PrefillUsersTable();
            //..
            db.SaveChanges();
        }
        #endregion
        #region Функционал общей инициализации.
        /** Функция DbIsNotInitialized
         * <summary>
         *  Функция осуществляет проверку заданности <br/>
         *   базе данных начальных значений.
         * </summary>
         * <returns>Возвращает флаг заданности.</returns>
         */
        private bool DbIsNotInitialized() {
            bool isNotInitialized = true;
            //..
            return isNotInitialized;
        }
        /** Функция Initialize
         * <summary>
         *  Функция осуществляет проверку заданности <br/>
         *   базе данных начальных значений. <br/>
         *   <br/>
         *  Если значения не заданы, осуществляется заполнение базы.
         * </summary>
         */
        private void Initialize() {
            if (DbIsNotInitialized()) {
                PrefillMainTables();
            }
        }
        /** Функция Initialize(ApplicationContext)
         * <summary>
         *  Статическая функция инкапсулирует <br/>
         *   создание объекта класса-инициализатора <br/>
         *   и проведение связанных операций. <br/>
         *  <br/>
         *  После создания экземпляра передаёт управление функции <see cref="Initialize()"/>. <br/>
         * </summary>
         * <param name="context">
         *  Контекст приложения предоставляющий интерфейс к базе данных.
         * </param>
         */
        public static void Initialize(ApplicationContext context) {
            var instance = new DbInitializer(context);
            instance.Initialize();
        }
        #endregion
    }
    #endregion
}