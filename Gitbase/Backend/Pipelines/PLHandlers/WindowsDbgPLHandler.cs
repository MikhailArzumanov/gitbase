using System.IO;

namespace Backend.Pipelines.PLHandlers {
    public class WindowsDbgPLHandler : IPipelineHandler {
        public void execGitInit(string dirpath) {
            var msgFormat = "Initializing git repository:\nDirpath: {0}";
            var message = string.Format(msgFormat, dirpath);
            Console.WriteLine(message);
        }
        public void createUser(string username, string password) {
            var msgFormat = "Creating user:\nUsername: {0}\nPassword: {1}";
            var message = string.Format(msgFormat, username, password);
            Console.WriteLine(message);
        }
        public void renameUser(string prevname, string newname) {
            var msgFormat = "Renaming user:\nPrevious name: {0}\nNew name: {1}";
            var message = string.Format(msgFormat, prevname, newname);
            Console.WriteLine(message);
        }
        public void changeUserPassword(string username, string password) {
            var msgFormat = "Changing user password:\nUsername: {0}\nPassword: {1}";
            var message = string.Format(msgFormat, username, password);
            Console.WriteLine(message);
        }
        public void removeUser(string username) {
            var msgFormat = "Removing user:\nUsername: {0}";
            var message = string.Format(msgFormat, username);
            Console.WriteLine(message);
        }
        public void createGroup(string groupname) {
            var msgFormat = "Creating group:\nGroupname: {0}";
            var message = string.Format(msgFormat, groupname);
            Console.WriteLine(message);
        }
        public void renameGroup(string prevname, string newname) {
            var msgFormat = "Renaming group:\nPrevious name: {0}\nNew name: {1}";
            var message = string.Format(msgFormat, prevname, newname);
            Console.WriteLine(message);
        }
        public void removeGroup(string groupname) {
            var msgFormat = "Removing group:\nGroupname: {0}";
            var message = string.Format(msgFormat, groupname);
            Console.WriteLine(message);
        }
        public void addUserToGroup(string groupname, string username) {
            var msgFormat = "Adding user to a group:\nGroupname: {0}\nUsername: {1}";
            var message = string.Format(msgFormat, groupname, username);
            Console.WriteLine(message);
        }
        public void removeUserFromGroup(string groupname, string username) {
            var msgFormat = "Removing user from a group:\nGroupname: {0}\nUsername: {1}";
            var message = string.Format(msgFormat, groupname);
            Console.WriteLine(message);
        }
        public void makeDir(string dirpath) {
            var msgFormat = "Creating directory:\nDirpath: {0}";
            var message = string.Format(msgFormat, dirpath);
            Console.WriteLine(message);
        }
        public void changeOwner(string path, string ownerName, bool andGroup = true) {
            var msgFormat = "Changing unit owner:\nPath: {0}\nOwner: {1}\nAnd group: {2}";
            var message = string.Format(msgFormat, path, ownerName, andGroup);
            Console.WriteLine(message);
        }
        public void changeGroup(string path, string groupname) {
            var msgFormat = "Changing unit group:\nPath: {0}\nGroupname: {1}";
            var message = string.Format(msgFormat, path, groupname);
            Console.WriteLine(message);
        }
        public void changeModifiers(string path, string rights) {
            var msgFormat = "Changing unit modifiers:\nPath: {0}\nRights: {1}";
            var message = string.Format(msgFormat, path, rights);
            Console.WriteLine(message);
        }
        public void move(string src, string dest) {
            var msgFormat = "Moving unit:\nSrc: {0}\nDest: {1}";
            var message = string.Format(msgFormat, src, dest);
            Console.WriteLine(message);
        }
        public void removeByPath(string path) {
            var msgFormat = "Removing unit:\nPath: {0}";
            var message = string.Format(msgFormat, path);
            Console.WriteLine(message);
        }
        public void writeFile(string dest, string fileText) {
            var msgFormat = "Writing file:\nPath: {0}\nData:\n{1}";
            var message = string.Format(msgFormat, dest, fileText);
            Console.WriteLine(message);
        }
    }
}
