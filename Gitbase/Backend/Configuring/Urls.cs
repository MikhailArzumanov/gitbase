namespace Backend.Configuring {
    public class Urls {
        public static string GetAppUrl(IConfiguration configuration) {
            var protocol = configuration["AppUrl:Protocol"] ?? "INVALID";
            var host     = configuration["AppUrl:Host"    ] ?? "INVALID";
            var port     = configuration["AppUrl:Port"    ] ?? "INVALID";
            var url = $"{protocol}://{host}:{port}";
            return url;
        }
    }
}
