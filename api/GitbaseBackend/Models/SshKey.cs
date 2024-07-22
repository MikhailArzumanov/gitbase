using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GitbaseBackend.Models {
    public class SshKey {
        [Key]
        public int    Id   { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Key  { get; set; } = String.Empty;

        [ForeignKey("UserId")]
        public User? User   { get; set; }
        public int   UserId { get; set; }
    }
}
