<script setup lang="ts">
  import { hide } from '@/shared/fns/notification-bar.fns';
import { InjectionKeys } from '@/shared/globals/injections.globals';
  import { HttpClient } from '@/shared/modules/http/http';
  import { ErrorsService } from '@/shared/services/common-services/errors.service';
  import { StorageService } from '@/shared/services/common-services/storage.service';
import { RepositoriesService } from '@/shared/services/repositories.service';
  import { UsersService } from '@/shared/services/users.service';
  import { provide } from 'vue';

  let errorsService = new ErrorsService();
  let storageService = new StorageService();

  let httpClient = new HttpClient(errorsService);

  let usersService = new UsersService(httpClient);
  let repositoriesService = new RepositoriesService(httpClient);

  provide(InjectionKeys.ERRORS  ,       errorsService);
  provide(InjectionKeys.USERS   ,        usersService);
  provide(InjectionKeys.STORAGE ,      storageService);
  provide(InjectionKeys.REPOS   , repositoriesService);

  let hideBar = hide;

</script>

<template>
  <RouterView />
  <div id="notificationBar" @click="hideBar"></div>
</template>

<style scoped>
  #notificationBar{
    text-align: center;
    padding   : 12px 40px;

    color         : black;
    background    : white;
    border        : 1px solid black;
    border-radius : 7px;
    
    
    position  : fixed;
    z-index   : 12;
    left      : 50vw;
    bottom    : 20px;
    transform : translateX(-50%);
    
    width: fit-content;

    visibility : hidden;
    opacity    : 0;
    transition : opacity 0.4s;
  }
</style>