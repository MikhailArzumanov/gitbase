<style scoped>
  @import url('./AuthPanel.css');
</style>
<script setup lang="ts">
  import { getEmptyUser, User } from '@/shared/models/user.model';
  import AuthForm from './forms/AuthForm.vue';
  import RegForm from './forms/RegForm.vue';
  import { getCurrentInstance, inject, onMounted } from 'vue';
  import type { UsersService } from '@/shared/services/users.service';
  import type { ErrorsService } from '@/shared/services/common-services/errors.service';
  import type { ComponentInternalInstance } from 'vue';
  import { notify } from '@/shared/fns/notification-bar.fns';
  import type { StorageService } from '@/shared/services/common-services/storage.service';
  import { Paths } from '@/shared/globals/paths.globals';
  import type { AuthResponse } from '@/shared/models/responses/auth.response';
  import { Messages } from '@/shared/globals/messages.globals';
  import { InjectionKeys } from '@/shared/globals/injections.globals';

  const errorsService  = inject(InjectionKeys.ERRORS ) as  ErrorsService;
  const usersService   = inject(InjectionKeys.USERS  ) as   UsersService;
  const storageService = inject(InjectionKeys.STORAGE) as StorageService;

  const AUTH_MODE = Symbol('authorization');
  const REG_MODE  = Symbol('registration');

  let mode = AUTH_MODE;
  let blockOpacity = 0.0;
  let user = getEmptyUser();
  let instance : ComponentInternalInstance | null = null;

  onMounted(() => {
    instance = getCurrentInstance();
    setTimeout(() => {
      blockOpacity = 1.0;
      reload();
    }, 100)
  })

  function reload(){
    instance?.proxy?.$forceUpdate();
  }

  function switchMode() : void {
    blockOpacity = 0.; reload(); 
    
    setTimeout(() => {
      mode = isRegistration() ? AUTH_MODE : REG_MODE;
      reload();
    }, 1000);

    setTimeout(() => { blockOpacity = 1.; reload(); }, 2000);
  }

  function isRegistration() : boolean {
    return mode == REG_MODE;
  }

  function getBlockClasses(){
    const base = 'auth-block';

    const concrete = isRegistration() ? 'reg' : 'auth';
    return `${base} ${base}_${concrete}`;
  }

  function handleError() : void { 
    const error = errorsService.getLastError() as string;
    console.log(error);
    notify(error);
  }

  function authorize() : void {
    usersService.auth(user.authname, user.password)
    .then((resp : AuthResponse | null)=>{
      if(resp == null) {handleError(); return;}
      usersService.setToken(resp.token);
      let message = Messages.AUTHORIZATION_SUCCESS;
      completeAuthorization(resp.user, message);
    })
  }

  function register() : void {
    usersService.register(user)
    .then((user: User | null)=>{
      if(user == null) {handleError(); return;}
      let message = Messages.REGISTRATION_SUCCESS;
      completeAuthorization(user, message);
    });
  }

  function completeAuthorization(user : User, message: string) : void {
    notify(message);
    storageService.setUser(user);
    let redirectPath = user.isAdmin ? Paths.ADMIN : Paths.PUBLIC;
    redirect(redirectPath);
  }

  function redirect(path: string) : void {
    instance?.proxy?.$router.push({ path: path});
  }

</script>

<template>
  <div class="auth-wrapper">
    <div :class="getBlockClasses()" 
        :style="{'opacity': blockOpacity }">
      <AuthForm v-if="!isRegistration()"
        :model="user"
        @switch-mode="switchMode()"
        @authorize="authorize"
      />
      <RegForm v-else 
        :model="user"
        @switch-mode="switchMode()"
        @register="register"
      />
    </div>
  </div>

</template>