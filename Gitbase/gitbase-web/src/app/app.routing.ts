import { createRouter, createWebHistory } from 'vue-router'
import { isAuthorized } from '@/shared/auth/isAuthorized'
import AuthPanel from './auth-panel/AuthPanel.vue'
import LayoutAdmin from './layout-admin/LayoutAdmin.vue'
import LayoutPublic from './layout-public/LayoutPublic.vue';
import { Paths } from '@/shared/globals/paths.globals';
import { adminRoutes } from './layout-admin/admin.routing';
import { publicRoutes } from './layout-public/public.routing';

function reduceDefaultRedirection(){
  let authorized = isAuthorized();
  return authorized ? Paths.PUBLIC : Paths.AUTHORIZATION;
}

const appRoutes = [
  {
    path     : '',
    redirect : reduceDefaultRedirection(),
  },
  {
    path      : Paths.AUTHORIZATION,
    component : AuthPanel,
  },
  {
    path      : Paths.ADMIN,
    component : LayoutAdmin,
    children  : adminRoutes,
  },
  {
    path      : Paths.PUBLIC,
    component : LayoutPublic,
    children  : publicRoutes,
  }
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: appRoutes,
})

export default router
