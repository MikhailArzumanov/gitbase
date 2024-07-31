import { createRouter, createWebHistory } from 'vue-router'
import LayoutAuth from './layout/auth/LayoutAuth.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path      : '/auth',
      component : LayoutAuth
    },
  ]
})

export default router
