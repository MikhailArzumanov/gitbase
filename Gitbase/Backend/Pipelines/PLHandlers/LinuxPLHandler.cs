using Backend.Constants;
using System.Diagnostics;

namespace Backend.Pipelines.PLHandlers {
    public class LinuxPLHandler : IPipelineHandler {

        public LinuxPLHandler(IConfiguration config) {
            var systemFamily = config[Configuration.SYSTEM_FAMILY_KEY] ?? "NONVALID";
            if (systemFamily != SystemTypes.LINUX_TYPE) {
                throw new Exception(ErrMsgs.SYSTEM_FAMILY_CONFIG_IS_NOT_VALID);
            }
        }

        void execLinuxCommand(string commandName, string args) {
            string result = "";
            using (Process proc = new Process()) {
                proc.StartInfo.FileName = $"/bin/{commandName}";
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
            Console.WriteLine($"Command: {commandName} {args}");
            Console.WriteLine($"Result: {result}");
            Console.WriteLine("-------------------------------");
        }
        public void execGitInit(string dirpath) {
            string result = "";
            using (Process proc = new Process()) {
                proc.StartInfo.FileName = "/bin/git";
                proc.StartInfo.Arguments = $"init {dirpath} --bare";
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
        private string execLinuxAndRead(string command) {
            string response = string.Empty;
            using (Process proc = new Process()) {
                proc.StartInfo.FileName = "/bin/bash";
                proc.StartInfo.Arguments = "-c \"" + command + "\"";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.Start();
                response = proc.StandardOutput.ReadToEnd();
                proc.WaitForExit();
            }
            return response;
        }

        public void createUser(string username, string password) {
            var args = string.Format(
                "-b /home -d /home/{0} -p {1} -s /bin/bash {0}", username, password
            );
            execLinuxCommand("useradd", args);
        }

        public void renameUser(string prevname, string newname) {
            var args = $"--login {newname} {prevname}";
            execLinuxCommand("usermod", args);
        }

        public void changeUserPassword(string username, string password) {
            var hashCommand = $"echo {password} | openssl passwd -1 -stdin";
            var hash = execLinuxAndRead(hashCommand);
            hash = hash.TrimEnd();

            hash = hash.Replace("\"", "\\\"");
            var args = $"-c \"usermod --password '{hash}' {username}\"";
            execLinuxCommand("bash", args);
        }

        public void removeUser(string username) {
            execLinuxCommand("userdel", username);
        }


        public void createGroup(string groupname) {
            execLinuxCommand("groupadd", groupname);
        }

        public void renameGroup(string prevname, string newname) {
            var args = $"-n {newname} {prevname}";
            execLinuxCommand("groupmod", args);
        }

        public void removeGroup(string groupname) {
            execLinuxCommand("groupdel", groupname);
        }

        public void addUserToGroup(string groupname, string username) {
            var args = $"-aG {groupname} {username}";
            execLinuxCommand("usermod", args);
        }

        public void removeUserFromGroup(string groupname, string username) {
            //..
        }

        public void makeDir(string dirpath) {
            execLinuxCommand("mkdir", dirpath);
        }
        public void changeOwner(string path, string ownerName, bool andGroup = true) {
            var args = $"{ownerName} {path}";
            execLinuxCommand("chown", args);
            if (andGroup) {
                execLinuxCommand("chown", $".{args}");
            }
        }
        public void changeGroup(string path, string groupname) {
            var args = $"-R {groupname} {path}";
            execLinuxCommand("chgrp", args);
        }
        public void changeModifiers(string path, string rights) {
            var args = $"-R {rights} {path}";
            execLinuxCommand("chmod", args);
        }

        public void move(string src, string dest) {
            var args = $"{src} {dest}";
            execLinuxCommand("mv", args);
        }

        public void removeByPath(string path) {
            execLinuxCommand("rm", $"-rf {path}");
        }

        public void writeFile(string dest, string fileText) {
            File.WriteAllText(dest, fileText);
        }
    }
}
