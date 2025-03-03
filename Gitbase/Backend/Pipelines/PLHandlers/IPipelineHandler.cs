namespace Backend.Pipelines.PLHandlers {
    public interface IPipelineHandler {

        public void execGitInit(string dirpath);
        public void createUser(string username, string password);
        public void renameUser(string prevname, string newname);
        public void changeUserPassword(string username, string password);
        public void removeUser(string username);
        public void createGroup(string groupname);
        public void renameGroup(string prevname, string newname);
        public void removeGroup(string groupname);
        public void addUserToGroup(string groupname, string username);
        public void removeUserFromGroup(string groupname, string username);
        public void makeDir(string dirpath);
        public void changeOwner(string path, string ownerName, bool andGroup = true);
        public void changeGroup(string path, string groupname);
        public void changeModifiers(string path, string rights);
        public void move(string src, string dist);
        public void removeByPath(string path);
        public void writeFile(string dest, string fileText);
    }
}