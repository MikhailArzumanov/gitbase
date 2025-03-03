<style scoped>
  @import url('../AuthPanel.css');
</style>

<script setup lang="ts">
  import FormField from './_FormField.vue';
  import { User } from '@/shared/models/user.model';
  import { FormFieldModel } from '../models/form-field.model';

  const   SWITCH_EVENT = 'switchMode' ;
  const REGISTER_EVENT = 'register'   ;

  defineProps({
    model: {
      type: User,
      required: true
    }
  })

  defineEmits([SWITCH_EVENT, REGISTER_EVENT]);

  const commonFields = [
    new FormFieldModel('Логин' , 'authname', 'text'    ),
    new FormFieldModel('Пароль', 'password', 'password'),
  ]

  const dataFields = [
    new FormFieldModel('Имя пользователя' , 'username', 'text'),
    new FormFieldModel('Электронная почта', 'email'   , 'text'),
    new FormFieldModel('О себе'           , 'about'   , 'text'),
    new FormFieldModel('Компания'         , 'company' , 'text'),
  ]

</script>

<template>
  <FormField v-for="field in commonFields"
    :model="model"
    :title="field.title"
    :model-key="field.modelKey"
    :field-type="field.fieldType"
  />
  <hr class="auth-hr"/>
  <FormField v-for="field in dataFields"
    :model="model"
    :title="field.title"
    :model-key="field.modelKey"
    :field-type="field.fieldType"
  />
  <hr class="auth-hr"/>
  <div class="auth-btn-wrapper">
    <button class="auth-btn" @click="$emit(REGISTER_EVENT)">
      Зарегистрироваться
    </button>
    <button class="auth-btn" @click="$emit(SWITCH_EVENT)">
      Авторизация
    </button>
  </div>
</template>