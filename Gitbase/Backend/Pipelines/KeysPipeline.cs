using Backend.Models;

namespace Backend.Pipelines {
    public class KeysPipeline : PipelineBase {
        public KeysPipeline(IConfiguration configuration) : base(configuration) { }
        public void UpdateAuthorizedKeys(string username, ICollection<SshKey> keys) {
            string compilatedKeys = String.Empty;
            foreach (SshKey key in keys) {
                compilatedKeys += key.Self;
            }

            var authorizedKeysFilepath = $"/home/{username}/.ssh/authorized_keys";
            handler.writeFile(authorizedKeysFilepath, compilatedKeys);
        }
    }
}
