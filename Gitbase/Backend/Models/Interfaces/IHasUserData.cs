namespace Backend.Models.Interfaces {
    public interface IHasUserData {
        public string UserName { get; set; }
        public string Email    { get; set; }
        public string About    { get; set; }
        public string Company  { get; set; }
        public string Links    { get; set; }
    }
}
