import { PublicPaths } from "@/shared/globals/paths.globals";
import Repositories from "./repositories/Repositories.vue";
import Repository from "./repository-page/RepositoryPage.vue";

export const publicRoutes = [
  {
    path     : '',
    redirect : PublicPaths.REPOSITORIES,
  },
  {
    path      : PublicPaths.REPOSITORIES,
    component : Repositories,
  },
  {
    path      : PublicPaths.REPOSITORY,
    component : Repository,
  }
]