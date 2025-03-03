using Backend.Data;
using Backend.Constants;
using Microsoft.EntityFrameworkCore;

namespace Backend.Configuring {
    public class DbConfigurator {
        public static void InitializeDB(WebApplication app) {
            using (var scope = app.Services.CreateScope()) {
                var services = scope.ServiceProvider;
                try {
                    var context = services.GetRequiredService<ApplicationContext>();
                    context.Database.Migrate();
                    DbInitializer.Initialize(context);
                } catch (Exception ex) {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, ErrMsgs.DB_INIT_ERROR_OCCURED);
                }
            }
        }
    }
}
