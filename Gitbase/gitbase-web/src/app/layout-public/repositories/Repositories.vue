<script setup lang="ts">
import { InjectionKeys } from '@/shared/globals/injections.globals';
import type { Page } from '@/shared/models/page.model';
import type { Repository } from '@/shared/models/repository.model';
import type { StorageService } from '@/shared/services/common-services/storage.service';
import type { RepositoriesService } from '@/shared/services/repositories.service';
import { inject, onMounted, ref } from 'vue';
import ReposTile from './repos-tile/ReposTile.vue';
import ReposView from './repos-view/ReposView.vue';
import type { User } from '@/shared/models/user.model';
import ReposCreate from './repos-create/ReposCreate.vue';

const storageService      = inject(InjectionKeys.STORAGE) as StorageService;
const repositoriesService = inject(InjectionKeys.REPOS)   as RepositoriesService;

let repositoriesList = ref([] as Repository[]);
let currentRepository = ref(null as Repository | null);
let user = ref(null as User | null);

onMounted(async () => {
  user.value = storageService.getUser();
  repositoriesService.getUsersRepositories(user.value?.id ?? 0, 1, 11)
  .then((repos: Page<Repository> | null) => {
    if(!repos){
      //..
      return;
    }
    repositoriesList.value = repos.entries;
  })
})

function openRepository(repository: Repository) {
  if(currentRepository.value?.id === repository.id) {
    currentRepository.value = null;
  } else {
    currentRepository.value = repository;
  }
}

</script>

<template>
<div class="pageGrid">
  <div class="leftColumn">
    <div class="header">
      Ваши репозитории
    </div>
    <div class="controlPanel">
      <div class="pagesBtn">
        &lt;
      </div>
      Страница 1/1
      <div class="pagesBtn">
        &gt;
      </div>
    </div>
    <div class="reposList">
      <ReposTile v-for="repository in repositoriesList" class="reposTile"
        :model="repository" 
        @click="() => openRepository(repository)"
      />
    </div>
  </div>
  <div class="rightColumn">
    <ReposView v-if="currentRepository && user" 
      :model="currentRepository" 
      :user-name="user.userName"
    />
    <ReposCreate v-else />
  </div>
</div>
</template>

<style lang="css" scoped>
  .pageGrid {
    display: grid;
    grid-template-columns: 1fr 4fr;
    grid-template-rows: 1fr;
    height: calc(100% - 2px);
  }

  .leftColumn {
    display: grid;
    grid-template-columns: 1fr;
    grid-template-rows: 32px 32px 1fr;

    border: 1px solid black;
  }
    .header {
      text-align: center;
      padding: 7px;
      border: 1px solid black;

      position: sticky;
      top: 0;
      background: white;
    }
    .controlPanel {
      display: grid;
      grid-template-columns: 1fr 4fr 1fr;
      text-align: center;
      padding: 7px;
      border: 1px solid black;

      position: sticky;
      top: 30px;
      background: white;
    }
      .pagesBtn {
        display: inline;
      }
    .reposList{
      display: grid;
      grid-template-columns: 1fr;
      grid-template-rows: repeat(12, fit-content(48px));

      border: 1px solid black;

      height: 100%;
      overflow-y: scroll;
    }

  .rightColumn {

    border: 1px solid black;
  }
</style>