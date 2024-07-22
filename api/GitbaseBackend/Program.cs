using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using GitbaseBackend.Data;

namespace GitbaseBackend {
    public class Program {

        delegate string HostBuilder(string address, string port, string protocol = "http");

        static HostBuilder GetHost = 
            (address, port, protocol) =>
                String.Format("{0}://{1}:{2}",
                    protocol, address, port);
        
        static string host = "http://localhost:1011";
        
        private static void HandleArgs(ref string[] args) {
            if(args.Length > 1) {
                Console.WriteLine("=====================");
                Console.WriteLine("Address  : " + args[1]);
                Console.WriteLine("Port     : " + args[2]);
                Console.WriteLine("=====================");
            }
            else {
                args = new string[3] { args.Length > 0 ? args[0] : "", 
                    "localhost", "1011" };

                Console.WriteLine("=====================");
                Console.WriteLine("For testing purposes,");
                Console.WriteLine("standard address");
                Console.WriteLine("and port were assigned");
                Console.WriteLine("http://localhost:1011");
                Console.WriteLine("=====================");
            }

            string address = args[1];
            string port    = args[2];

            host = GetHost(address, port);
        }

        private static IWebHost SetEnv(string[] args) {
            var host = CreateWebHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope()) {
                var services = scope.ServiceProvider;
                try {
                    var context = services.GetRequiredService<ApplicationContext>();
                    context.Database.Migrate();
                    DbInitializer.Initialize(context);
                }
                catch (Exception ex) {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "При инициализации базы данных была встречена ошибка.");
                }
            }
            return host;
        }

        public static void Main(string[] args) {
            HandleArgs(ref args);
            SetEnv(args).Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args).UseStartup<Startup>()
                .UseUrls(host);
    }
}