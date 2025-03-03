<script setup lang="ts">
import WelcomeItem from './WelcomeItem.vue'
import Icon from './icons/Icon.vue'
import { IconModel, icons } from './icons/icons'

class Item {
  icon        : IconModel;
  heading     : string;
  containemnt : string;
  constructor(icon: any, heading: string, containemnt: string){
    this.icon        = icon        ;
    this.heading     = heading     ;
    this.containemnt = containemnt ;
  }
}

let headings = [
  'Documentation' ,
  'Tooling'       ,
  'Ecosystem'     ,
  'Community'     ,
  'Support'       ,
]

let data = [0,1,2,3,4].map(
  x => new Item(icons[x], headings[x], headings[x]+' containment.')
);

</script>

<template>
  <WelcomeItem v-for="item in data">
    <template #icon>
      <Icon
        v-bind:w="item.icon.width"
        v-bind:h="item.icon.height"
        v-bind:pathD="item.icon.path"
      />
    </template>
    <template #heading>
      {{ item.heading }}
    </template>
    {{ item.containemnt }}
  </WelcomeItem>

</template>
