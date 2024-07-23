using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GitbaseBackend.Models {
    public class Repository {
        [Key]
        public int    Id   { get; set; }
        public string Name { get; set; } = "";

        [ForeignKey("OwnerId")]
        public User? Owner   { get; set; }
        public int   OwnerId { get; set; }

        public bool  IsPrivate { get; set; }

        public List<User> Collaborators { get; set; } = new List<User>();
    }
}
