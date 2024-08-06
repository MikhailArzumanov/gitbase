<style scoped>
  @import url('../AuthPanel.css');
</style>

<script setup lang="ts">
  import FormField from './_FormField.vue';
  import { FormFieldModel } from '../models/form-field.model';
  import { User } from '@/shared/models/user.model';

  const SWITCH_EVENT    = 'switchMode';
  const AUTHORIZE_EVENT = 'authorize' ;

  defineProps({
    model: {
      type: User,
      required: true
    }
  })

  defineEmits([SWITCH_EVENT, AUTHORIZE_EVENT]);

  let fields = [
    new FormFieldModel('Логин' , 'authname', 'text'    ),
    new FormFieldModel('Пароль', 'password', 'password'),
  ]

</script>

<template>
  <FormField v-for="field in fields"
    :model="model"
    :title="field.title"
    :model-key="field.modelKey"
    :field-type="field.fieldType"
  />
  <hr class="auth-hr"/>
  <div class="auth-btn-wrapper">
    <button class="auth-btn" @click="$emit(AUTHORIZE_EVENT)">
      Авторизоваться
    </button>
    <button class="auth-btn" @click="$emit(SWITCH_EVENT)">
      Регистрация
    </button>
  </div>
</template>