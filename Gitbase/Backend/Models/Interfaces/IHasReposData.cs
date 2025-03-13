namespace Backend.Models.Interfaces {
    public interface IHasReposData {
        public string Name        { get; set; }
        public string Description { get; set; }
        public bool   IsPrivate   { get; set; } 
        public int    OwnerId     { get; set; }
    }
}
