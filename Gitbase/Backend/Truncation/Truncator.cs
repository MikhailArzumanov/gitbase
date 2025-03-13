using Backend.Models;

namespace Backend.Truncation {
    public class Truncator {
        public static User Truncate(User user) {
            user.OwnedRepositories        = [];
            user.ParticipationRepositories = [];
            user.SshKeys                  = [];
            user.Password = String.Empty;
            return user;
        }
        public static SshKey Truncate(SshKey theKey) {
            theKey.User = null;
            return theKey;
        }
        public static Repository Truncate(Repository repository) {
            repository.Owner = null;
            repository.Participators = [];
            return repository;
        }
    }
}
