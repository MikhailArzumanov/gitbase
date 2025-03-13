using gitbase_desktop.Models;

namespace gitbase_desktop.Data {
    public class ApplicationContext {
        public Token AuthorizationToken { get; set; } = null;
        public User  CurrentUser        { get; set; } = null;
        public ApplicationContext() {}
    }
}
