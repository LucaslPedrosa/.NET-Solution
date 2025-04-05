// src/router/index.ts
import { createRouter, createWebHistory } from 'vue-router'
import JokeList from '@/pages/JokeList.vue'
import Dashboard from '@/pages/Dashboard.vue'

const routes = [
  {
    path: '/',
    name: 'Home',
    component: JokeList
  },
  {
    path: '/dashboard',
    name: 'Dashboard',
    component: Dashboard
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
