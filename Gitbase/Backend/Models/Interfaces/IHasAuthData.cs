namespace Backend.Models.Interfaces {
    public interface IHasAuthData {
        public string Authname { get; set; }
        public string Password { get; set; }
    }
}
