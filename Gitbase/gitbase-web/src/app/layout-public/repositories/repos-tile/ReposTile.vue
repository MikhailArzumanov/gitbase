<script setup lang="ts">
import { Repository } from '@/shared/models/repository.model';

defineProps({
  model: {
    type: Repository,
    required: true
  }
})


</script>

<template>
  <div class="reposTile">
    <p class="reposName">
      {{ model.name }}
    </p>
    <p class="reposDescription">
      {{ model.description }}
    </p>
  </div>
</template>

<style lang="css" scoped>
  * {
    margin: 0;
  }
  .reposTile {
    display: grid;
    grid-template-rows: 40px 80px;
    grid-template-columns: 1fr;  
    
    border: 1px solid black;
    padding: 7px;

    cursor: pointer;
  }
  .reposName {
    font-size: 24px;
  }
  .reposDescription {
    font-size: 12px;
  }
</style>