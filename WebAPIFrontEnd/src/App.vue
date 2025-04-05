<template>
  <div class="bg-white text-black dark:bg-black dark:text-white min-h-screen">
    <topHeader
      :is-dark="isDark"
      :is-mock="isMock"
      :mock-label="mockLabel"
      @toggle-dark="toggleDark"
    />
    <router-view />
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import topHeader from './components/TopHeader.vue'

const isDark = ref(false)
const isMock = import.meta.env.VITE_USE_MOCK === 'true'
const mockLabel = isMock ? 'Mock mode on' : 'Mock mode off'

function toggleDark() {
  isDark.value = !isDark.value
  document.documentElement.classList.toggle('dark', isDark.value)
  localStorage.setItem('dark-mode', String(isDark.value))
}

onMounted(() => {
  const saved = localStorage.getItem('dark-mode')
  if (saved === 'true') {
    document.documentElement.classList.add('dark')
    isDark.value = true
  }
})
</script>

<style>
body {
  font-family: sans-serif;
}
</style>