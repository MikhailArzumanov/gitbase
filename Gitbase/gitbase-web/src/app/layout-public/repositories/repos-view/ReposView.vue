<script setup lang="ts">
import { notify } from '@/shared/fns/notification-bar.fns';
import { InjectionKeys } from '@/shared/globals/injections.globals';
import { Repository } from '@/shared/models/repository.model';
import type { RepositoriesService } from '@/shared/services/repositories.service';
import { inject } from 'vue';
const props = defineProps({
  model: {
    type: Repository,
    required: true
  },
  userName: {
    type: String,
    required: true
  }
})

const repositoriesService = inject(InjectionKeys.REPOS) as RepositoriesService;

function getSshLink(userName: string, repName: string) : string {
  return `${userName}@gitbase.ru:/git/${userName}/${repName}`;
}

async function deleteRepository() {
  let repName = props.model?.name;
  let msg = `Вы уверены, что хотите удалить репозиторий '${repName}'`;
  let answer = confirm(msg);
  if(!answer) return;
  let res = await repositoriesService.deleteSelfRepository(props.model.id);
  if(!res) {
    notify("При удалении репозитория произошла ошибка.");
    return;
  }
  notify("Репозиторий был успешно удалён.");
}

</script>

<template>
<div class="container">
  <p class="repositoryName">
    {{ model.name }}
  </p>
  <p class="sideInfo">
    Видимость: {{ model.isPrivate ? "частный" : "публичный" }}
  </p>
  <p class="sideInfo">
    SSH-ссылка на репозиторий: {{ getSshLink(userName, model.name) }}
  </p>
  <p class="repositoryDescription">
    {{ model.description }}
  </p>
  <div class="btn" @click="deleteRepository">
    Удалить репозиторий
  </div>
</div>
</template>

<style lang="css" scoped>
  * {
    margin: 0;
  }
  .container{
    padding: 12px;
  }
    .repositoryName {
      font-size: 48px;
    }
    .sideInfo{
      font-size: 16px;
    }
    .repositoryDescription {
      font-size: 24px;
      margin-top: 12px;
    }
    .btn {
      border: 1px solid black;
      padding: 12px;
      text-align: center;
      width: 480px;
    }
</style>