namespace GitbaseBackend.Models {
    public class Token {
        public string   Self   { get; set; } = String.Empty;
        public string[] Roles  { get; set; } = new string[0];
        public int      UserId { get; set; }
    }
}
