<script setup lang="ts">
import { notify } from '@/shared/fns/notification-bar.fns';
import { InjectionKeys } from '@/shared/globals/injections.globals';
import { Repository } from '@/shared/models/repository.model';
import type { StorageService } from '@/shared/services/common-services/storage.service';
import type { RepositoriesService } from '@/shared/services/repositories.service';
import { inject } from 'vue';

const repositoriesService = inject(InjectionKeys.REPOS) as RepositoriesService;
const storageService = inject(InjectionKeys.STORAGE) as StorageService;

let user = storageService.getUser();

const model = new Repository(0, user?.id ?? 0, true, "НовыйРепозиторий", "...");


async function createRepository(){
  let response = await repositoriesService.createSelfRepository(model);
  if(!response) {
    notify("При создании репозитория произошла ошибка.");
  } else {
    notify("Репозиторий успешно создан.");
    setTimeout(() => location.reload, 1000);
  }
}

</script>

<template>
<div class="reposCreationForm">
  <p class="label">Имя репозитория:</p>
  <input class="name-field" type="text" v-model="model.name" />
  
  <p class="label-desc">Описание:</p>
  <textarea class="desc-field" v-model="model.description">
  </textarea>

  <p class="label">Частный:</p>
  <input class="private-field" type="checkbox" v-model="model.isPrivate" />

  <div class="btn" @click="createRepository">
    Создать
  </div>
</div>
</template>

<style lang="css" scoped>
* {
  margin: 0;
}
.reposCreationForm {
  padding: 12px;
  display: grid;
  grid-template-columns: 200px 240px;
  grid-template-rows: 24px 24px 120px 24px;

  row-gap: 12px;
}
  .label{
    
  }
  .label-desc{
    grid-column-start: 1;
    grid-column-end: 3;
  }
  .desc-field {
    grid-column-start: 1;
    grid-column-end: 3;
  }
  .btn {
    grid-column-start: 1;
    grid-column-end: 3;
    border: 1px solid black;
    padding: 12px;
    text-align: center;
    font-size: 34px;
  }
</style>