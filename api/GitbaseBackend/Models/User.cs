using System.ComponentModel.DataAnnotations;

namespace GitbaseBackend.Models {
    public class User {
        [Key]
        public int    Id       { get; set; }
        public string Authname { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;

        public string Username { get; set; } = String.Empty;
        public string Email    { get; set; } = String.Empty;
        public string About    { get; set; } = String.Empty;
        public string Company  { get; set; } = String.Empty;
        public string Links    { get; set; } = String.Empty;


        public List<SshKey> SshKeys { get; set; } = new List<SshKey>();


        public List<Repository> OwnedRepositories { get; set; } = new List<Repository>();

        public List<Repository> CollaboratedRepositories { get; set; } = new List<Repository>();
    }
}
