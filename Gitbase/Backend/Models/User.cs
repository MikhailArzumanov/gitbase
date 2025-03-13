using Backend.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models {
    public class User : IHasAuthUserData {
        [Key]
        public int    Id       { get; set; }
        public string Authname { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;

        public string UserName { get; set; } = String.Empty;
        public string Email    { get; set; } = String.Empty;
        public string About    { get; set; } = String.Empty;
        public string Company  { get; set; } = String.Empty;
        public string Links    { get; set; } = String.Empty;


        public ICollection<SshKey    > SshKeys                   { get; set; } = [];
        public ICollection<Repository> OwnedRepositories         { get; set; } = [];
        public ICollection<Repository> ParticipationRepositories { get; set; } = [];
    }
}
