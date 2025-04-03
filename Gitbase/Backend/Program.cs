using Backend.Configuring;
using Backend.Constants;

namespace Backend {
    public class Program {
        
        private static WebApplication BuildApplication(string[] args) {
            var builder = WebApplication.CreateBuilder(args);
            ServicesConfigurator.Configure(
                services: builder.Services,
                configuration: builder.Configuration
            );
            var app = builder.Build();
            return app;
        }
        
        private static void ConfigureApplication(WebApplication app) {
            bool isDevelopmentEnv = app.Environment.IsDevelopment();
            AppConfigurator.Configure(app,
                configuration: app.Configuration,
                isDevelopmentEnv: true
            );
        }
        private static void RunApplication(WebApplication app) {
            var appUrl = Urls.GetAppUrl(app.Configuration);
            app.Urls.Add(appUrl);
            app.Run();
        }
        private static void StartApplication(string[] args) {
            var app = BuildApplication(args);
            DbConfigurator.InitializeDB(app);
            ConfigureApplication(app);
            RunApplication(app);
        }
        public static void Main(string[] args) {
            StartApplication(args);
        }
    }
}
