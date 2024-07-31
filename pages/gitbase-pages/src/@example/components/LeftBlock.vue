<script setup lang="ts">
defineProps<{
  msg: string
}>()
</script>

<template>
  <div class="greetings">
    <h1 class="green">{{ msg }}</h1>
    <h3>
      Example text.
    </h3>
  </div>
</template>

<style scoped>
.greetings {
  text-align: left;
}
</style>
