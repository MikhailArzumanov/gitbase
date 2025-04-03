<script setup lang="ts">
  import { User } from '@/shared/models/user.model';

  defineProps({
    model: {
      type: User,
      required: true,
    },
    title: {
      type: String,
      required: true,
    },
    modelKey: {
      type: String,
      required: true,
    },
    fieldType: String
  })

</script>
<template>
  <div class="auth-grid-row">
    <p class="auth-field-title">
      {{title}}:
    </p>
    <input class="auth-field" :type="fieldType"
      v-model="model[modelKey]"
    />
  </div>
</template>
<style scoped>
  @import url('../AuthPanel.css');
</style>