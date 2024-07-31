<script setup>
  const props = defineProps({
    pathD  : String,
    w      : Number,
    h      : Number,
  });
</script>
<template>
  <svg xmlns="http://www.w3.org/2000/svg" 
    v-bind:width="w" 
    v-bind:height="h"
    fill="currentColor"
  >
    <path 
      v-bind:d="pathD" 
    />
  </svg>
</template>