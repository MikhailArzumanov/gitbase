namespace Backend.Pipelines {
    public class UsersPipelines : PipelineBase {
        public UsersPipelines(IConfiguration configuration) : base(configuration) { }

        private void ValidateUsername(ref string username) {
            username = username.Replace(' ', '_');
        }

        public void CreateUser(string username, string password) {
            ValidateUsername(ref username);
            var homedir = String.Format("/home/{0}", username);
            handler.makeDir(homedir);
            handler.makeDir(homedir + "/.ssh");

            handler.createUser(username, password);
            handler.changeUserPassword(username, password);

            handler.changeOwner(homedir, username);

            var gitdir = String.Format("/git/{0}", username);
            handler.makeDir(gitdir);
            handler.changeOwner(gitdir, username);
        }

        public void RenameUser(string oldUsername, string newUsername) {
            ValidateUsername(ref oldUsername);
            ValidateUsername(ref newUsername);
            handler.renameUser(oldUsername, newUsername);

            string homedirFormat = "/home/{0}",
                   prevHomedir = String.Format(homedirFormat, oldUsername),
                   newHomedir = String.Format(homedirFormat, newUsername);
            handler.move(prevHomedir, newHomedir);

            string gitdirFormat = "/git/{0}",
                   prevGitdir = String.Format(gitdirFormat, oldUsername),
                   newGitdir = String.Format(gitdirFormat, newUsername);
            handler.move(prevGitdir, newGitdir);
        }

        public void RemoveUser(string username) {
            ValidateUsername(ref username);
            var homedir = String.Format("/home/{0}", username);
            var gitdir = String.Format("/git/{0}", username);
            handler.removeUser(username);
            handler.removeByPath(homedir);
            handler.removeByPath(gitdir);
        }

        public void ChangeUserPassword(string username, string newPassword) {
            ValidateUsername(ref username);
            handler.changeUserPassword(username, newPassword);
        }
    }
}
