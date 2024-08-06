using GitbaseBackend.Models;
using System.Diagnostics;

namespace GitbaseBackend.Utils {
    public class Pipelines {

        string systemFamily = String.Empty;

        public Pipelines(IConfiguration config) {
            systemFamily = config["System"];
        }


        void execLinuxCommand(string commandName, string args) {
            if(systemFamily != "Linux") {
                Console.WriteLine("Command: {0} {1}", commandName, args);
                return;
            }
            string result = "";
            using (Process proc = new Process()) {
                proc.StartInfo.FileName = "/bin/"+commandName;
                proc.StartInfo.Arguments = args;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.Start();

                result += proc.StandardOutput.ReadToEnd();
                result += proc.StandardError.ReadToEnd();

                proc.WaitForExit();
            }
            Console.WriteLine("-------------------------------");
            Console.WriteLine("Command: {0} {1}", commandName, args);
            Console.WriteLine("Result: {0}", result);
            Console.WriteLine("-------------------------------");
        }
        void execGitInit(string dirpath) {
            if (systemFamily != "Linux") {
                Console.WriteLine("Git init at '{0}'", dirpath);
                return;
            }
            string result = "";
            using (Process proc = new Process()) {
                proc.StartInfo.FileName = "/bin/git";
                proc.StartInfo.Arguments = String.Format("init {0} --bare", dirpath);
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.Start();

                result += proc.StandardOutput.ReadToEnd();
                result += proc.StandardError.ReadToEnd();

                proc.WaitForExit();
            }
            Console.WriteLine("-------------------------------");
            Console.WriteLine("Git init at '{0}'", dirpath);
            Console.WriteLine("Result: {0}", result);
            Console.WriteLine("-------------------------------");
        }
        string execLinuxAndRead(string command) {
            if (systemFamily != "Linux") {
                return String.Empty;
            }
            string response = String.Empty;
            using(Process proc = new Process()) {
                proc.StartInfo.FileName = "/bin/bash";
                proc.StartInfo.Arguments = "-c \""+command+"\"";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.Start();
                response = proc.StandardOutput.ReadToEnd();
                proc.WaitForExit();
            }
            return response;
        }

        void createUserLinux(string username, string password) {
            var args = String.Format("-b /home -d /home/{0} -p {1} -s /bin/bash {0}", username, password);
            execLinuxCommand("useradd", args);
        }

        void renameUserLinux(string prevname, string newname) {
            var args = String.Format("--login {1} {0}", prevname, newname);
            execLinuxCommand("usermod", args);
        }

        void changeUserPasswordLinux(string username, string password) {
            if (systemFamily != "Linux") {
                Console.WriteLine("Password changing.");
            }
            var hashCommand = String.Format("echo {0} | openssl passwd -1 -stdin", password);
            var hash = execLinuxAndRead(hashCommand);
            hash = hash.TrimEnd();

            hash = hash.Replace("\"", "\\\"");
            var argsFormat = "-c \"usermod --password '{1}' {0}\"";
            var args = String.Format(argsFormat, username, hash);
            execLinuxCommand("bash", args);
        }

        void removeUserLinux(string username) {
            execLinuxCommand("deluser", username);
        }


        void createGroupLinux(string groupname) {
            execLinuxCommand("groupadd", groupname);
        }

        void renameGroupLinux(string prevname, string newname) {
            var args = String.Format("-n {1} {0}", prevname, newname);
            execLinuxCommand("groupmod", args);
        }

        void removeGroupLinux(string groupname) {
            execLinuxCommand("groupdel", groupname);
        }

        void addUserToGroupLinux(string groupname, string username) {
            var args = String.Format("-aG {0} {1}", groupname, username);
            execLinuxCommand("usermod", args);
        }

        void removeUserFromGroupLinux(string groupname, string username) {
            //..
        }

        void makeDirLinux(string dirpath) {
            execLinuxCommand("mkdir", dirpath);
        }
        void changeOwnerLinux(string path, string ownerName, bool andGroup = true) {
            var args = String.Format("{0} {1}", ownerName, path);
            execLinuxCommand("chown", args);
            if (andGroup) {
                execLinuxCommand("chown", "."+args);
            }
        }
        void changeGroupLinux(string path, string groupname){
            var args = String.Format("-R {0} {1}", groupname, path);
            execLinuxCommand("chgrp", args);
        }
        void changeModifiersLiniux(string path, string rights) {
            var args = String.Format("-R {1} {0}", path, rights);
            execLinuxCommand("chmod", args);
        }

        void moveLinux(string src, string dist){
            var args = String.Format("{0} {1}", src, dist);
            execLinuxCommand("mv", args);
        }

        void removeByPathLinux(string path) {
            execLinuxCommand("rm", "-rf " + path);
        }

        public void CreateRepository(string repositoryName, string ownerName, bool isPublic) {
            var groupname = String.Format("{0}__{1}", ownerName, repositoryName);
            var dirpath = String.Format("/git/{0}/{1}", ownerName, repositoryName);
            createGroupLinux(groupname);
            addUserToGroupLinux(groupname, ownerName);
            makeDirLinux(dirpath);
            execGitInit(dirpath);
            changeModifiersLiniux(dirpath, isPublic ? "775" : "770");
            changeOwnerLinux(dirpath, ownerName, false);
            changeGroupLinux(dirpath, groupname);
        }

        public void RenameRepository(string repositoryOldName, string repositoryNewName, string ownerName) {
            string repoDirFormat = "/git/{0}/{1}",
                   prevRepoDir   = String.Format(repoDirFormat, ownerName, repositoryOldName),
                   newRepoDir    = String.Format(repoDirFormat, ownerName, repositoryNewName);
            
            string groupFromat = "{0}__{1}",
                   prevGroup   = String.Format(groupFromat, ownerName, repositoryOldName),
                   newGroup    = String.Format(groupFromat, ownerName, repositoryNewName);
            
            renameGroupLinux(prevGroup, newGroup);
            moveLinux(prevRepoDir, newRepoDir);
        }

        public void RemoveRepository(string repositoryName, string ownerName) {
            var dirpath = String.Format("/git/{0}/{1}", ownerName, repositoryName);
            var groupname = String.Format("{0}__{1}", ownerName, repositoryName);

            removeGroupLinux(groupname);
            removeByPathLinux(dirpath);
        }


        public void CreateUser(string username, string password) {
            var homedir = String.Format("/home/{0}", username);
            makeDirLinux(homedir);
            makeDirLinux(homedir+"/.ssh");

            createUserLinux(username, password);
            changeUserPasswordLinux(username, password);

            changeOwnerLinux(homedir, username);

            var gitdir = String.Format("/git/{0}", username);
            makeDirLinux(gitdir);
            changeOwnerLinux(gitdir, username);
        }

        public void RenameUser(string oldUsername, string newUsername) {
            renameUserLinux(oldUsername, newUsername);

            string homedirFormat = "/home/{0}",
                   prevHomedir   = String.Format(homedirFormat, oldUsername),
                   newHomedir    = String.Format(homedirFormat, newUsername);
            moveLinux(prevHomedir, newHomedir);

            string gitdirFormat = "/git/{0}",
                   prevGitdir   = String.Format(gitdirFormat, oldUsername),
                   newGitdir    = String.Format(gitdirFormat, newUsername);
            moveLinux(prevGitdir, newGitdir);
        }

        public void RemoveUser(string username) {
            var homedir = String.Format("/home/{0}", username);
            var gitdir  = String.Format("/git/{0}" , username);
            removeUserLinux(username);
            removeByPathLinux(homedir);
            removeByPathLinux(gitdir);
        }

        public void ChangeUserPassword(string username, string newPassword) {
            changeUserPasswordLinux(username, newPassword);
        }

        public void UpdateAuthorizedKeys(string username, ICollection<SshKey> keys) {
            string compilatedKeys = String.Empty;
            foreach (SshKey key in keys) {
                compilatedKeys += key.Key;
            }

            var authorizedKeysFilepath = "/home/" + username + "/.ssh/authorized_keys";

            File.WriteAllText(authorizedKeysFilepath, compilatedKeys);
        }

    }
}
