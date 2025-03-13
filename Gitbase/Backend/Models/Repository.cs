using Backend.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models {
    public class Repository : IHasReposData {
        [Key]
        public int    Id          { get; set; }
        public string Name        { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public bool   IsPrivate   { get; set; } 

        [ForeignKey("OwnerId")]
        public User? Owner   { get; set; }
        public int   OwnerId { get; set; }

        public ICollection<User> Participators { get; set; } = [];
    }
}
