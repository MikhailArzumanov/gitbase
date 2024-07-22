using GitbaseBackend.Models;
using System.Diagnostics;

namespace GitbaseBackend.Utils {
    public class Pipelines {

        const string basepath = "D:\\Projects\\gitbase\\pipelines\\";

        string systemFamily = String.Empty;

        public Pipelines(IConfiguration config) {
            systemFamily = config["System"];

            for(int i = 0; i < scriptKeys.Length; i++) {
                string key      = scriptKeys[i]      ;
                string filename = scriptFileNames[i] ;

                scripts[key] = File.ReadAllText(basepath+filename);
            }
        }

        Dictionary<string, string> scripts = new Dictionary<string, string>();
        string[] scriptFileNames = new string[] {
            "createRepository.sh", "renameRepository.sh", "removeRepository.sh",
            "createUser.sh"      , "renameUser.sh"      , "removeUser.sh"      , "changeUserPassword.sh" ,

        };
        string[] scriptKeys = new string[] {
            "createRepository", "renameRepository", "removeRepository",
            "createUser"      , "renameUser"      , "removeUser"      , "changeUserPassword" ,

        };

        void execLinuxCommand(string command) {
            command.Replace("\"", "\\\"");
            string result = "";
            using (Process proc = new Process()) {
                proc.StartInfo.FileName = "/bin/bash";
                proc.StartInfo.Arguments = "-c \" " + command + " \"";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.Start();

                result += proc.StandardOutput.ReadToEnd();
                result += proc.StandardError.ReadToEnd();

                proc.WaitForExit();
            }
            //Console.WriteLine(result);
        }

        void execCommand(string command) {
            if(systemFamily == "Windows") {
                Console.WriteLine("Executing command: {0}", command);
            }
            else if (systemFamily == "Linux") {
                execLinuxCommand(command);
            }
            else {
                Console.WriteLine("Встречено некорректное значение конфигурации. (Поле 'System') ");
            }
        }


        void execRepoScript(string script, string repositoryName, string ownerName) {
            
            script = script.Replace("%USERNAME%" ,      ownerName);
            script = script.Replace("%REPO_NAME%", repositoryName);

            var commands = script.Split('\n');
            foreach (var command in commands) {
                execCommand(command);
            }
        }

        public void CreateRepository(string repositoryName, string ownerName) {
            var scriptName = "createRepository";
            var script = scripts[scriptName];
            execRepoScript(script, repositoryName, ownerName);
        }

        public void RenameRepository(string repositoryOldName, string repositoryNewName, string ownerName) {
            var scriptName = "renameRepository";
            var script = scripts[scriptName];
            script = script.Replace("%PREV_NAME%", repositoryOldName);
            execRepoScript(script, repositoryNewName, ownerName);
        }

        public void RemoveRepository(string repositoryName, string ownerName) {
            var scriptName = "removeRepository";
            var script = scripts[scriptName];
            execRepoScript(script, repositoryName, ownerName);
        }


        public void CreateUser(string username, string password) {
            var scriptName = "createUser";
            var script = scripts[scriptName];

            script = script.Replace("%USERNAME%", username);
            script = script.Replace("%PASSWORD%", password);
            
            var commands = script.Split('\n');
            foreach (var command in commands) {
                execCommand(command);
            }
        }

        public void RenameUser(string oldUsername, string newUsername) {
            var scriptName = "renameUser";
            var script = scripts[scriptName];

            script = script.Replace("%USERNAME%" , newUsername);
            script = script.Replace("%PREV_NAME%", oldUsername);
            
            var commands = script.Split('\n');
            foreach (var command in commands) {
                execCommand(command);
            }
        }

        public void RemoveUser(string username) {
            var scriptName = "renameUser";
            var script = scripts[scriptName];

            script = script.Replace("%USERNAME%" , username);
            
            var commands = script.Split('\n');
            foreach (var command in commands) {
                execCommand(command);
            }
        }

        public void ChangeUserPassword(string username, string newPassword) {
            var scriptName = "changeUserPassword";
            var script = scripts[scriptName];

            script = script.Replace("%USERNAME%" , username);
            script = script.Replace("%PASSWORD%" , newPassword);

            var commands = script.Split('\n');
            foreach (var command in commands) {
                execCommand(command);
            }
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
