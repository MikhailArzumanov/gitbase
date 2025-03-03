using Backend.Models;

namespace Backend.Pipelines {
    public class RepositoriesPipelines : PipelineBase {

        public RepositoriesPipelines(IConfiguration configuration) 
            : base(configuration) {}

        public void CreateRepository(
            string  repositoryName ,
            string  ownerName      ,
            bool    isPublic
        ) {
            var groupname = String.Format("{0}__{1}", ownerName, repositoryName);
            var dirpath = String.Format("/git/{0}/{1}", ownerName, repositoryName);
            handler.createGroup(groupname);
            handler.addUserToGroup(groupname, ownerName);
            handler.makeDir(dirpath);
            handler.execGitInit(dirpath);
            handler.changeModifiers(dirpath, isPublic ? "775" : "770");
            handler.changeOwner(dirpath, ownerName, false);
            handler.changeGroup(dirpath, groupname);
        }

        public void RenameRepository(
            string repositoryOldName, 
            string repositoryNewName, 
            string ownerName
        ) {
            string repoDirFormat = "/git/{0}/{1}",
                   prevRepoDir = String.Format(repoDirFormat, ownerName, repositoryOldName),
                   newRepoDir = String.Format(repoDirFormat, ownerName, repositoryNewName);

            string groupFromat = "{0}__{1}",
                   prevGroup = String.Format(groupFromat, ownerName, repositoryOldName),
                   newGroup = String.Format(groupFromat, ownerName, repositoryNewName);

            handler.renameGroup(prevGroup, newGroup);
            handler.move(prevRepoDir, newRepoDir);
        }

        public void RemoveRepository(string repositoryName, string ownerName) {
            var dirpath = String.Format("/git/{0}/{1}", ownerName, repositoryName);
            var groupname = String.Format("{0}__{1}", ownerName, repositoryName);

            handler.removeGroup(groupname);
            handler.removeByPath(dirpath);
        }
    }
}
