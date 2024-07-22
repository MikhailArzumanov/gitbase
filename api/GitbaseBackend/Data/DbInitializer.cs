namespace GitbaseBackend.Data {
    public class DbInitializer {
        private static ApplicationContext db;

        private static void PrefillMainTables() {
            //...
            db.SaveChanges();
        }
        public static void Initialize(ApplicationContext context) {
            db = context;
            
        }
    }
}
