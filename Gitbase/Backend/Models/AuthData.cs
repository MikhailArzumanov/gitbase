using Backend.Models.Interfaces;

namespace Backend.Models {
    public class AuthData : IHasAuthData {
        public string Authname { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
    }
}
