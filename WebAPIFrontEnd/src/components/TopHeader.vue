<template>
  <!-- HEADER -->
  <header
    class="fixed top-0 left-0 w-full bg-white/90 text-black dark:bg-zinc-700/80 dark:text-white backdrop-blur-md shadow z-50 transition-colors duration-300">
    <div class="max-w-4xl mx-auto px-6 py-4 flex justify-between items-center">
      <h1 class="text-3xl font-bold font-bebas">💬 Jokes Database</h1>

      <div class="flex gap-4 items-center">
        <!-- Botão de Estatísticas -->
        <!-- Toggle Main/Stats button -->
        <button @click="navigate"
          class="text-sm bg-indigo-600 hover:bg-indigo-700 text-white px-3 py-1 rounded shadow transition">
          {{ isDashboard ? '🏠 Main' : '📊 Stats' }}
        </button>

        <!-- Dark Mode Toggle -->
        <button @click="toggleDark" class="relative flex items-center gap-2 text-sm px-4 py-2 rounded shadow transition-all duration-300
         bg-gray-200 text-black hover:bg-gray-300
         dark:bg-gray-700 dark:text-white dark:hover:bg-gray-600">
          <span class="inline-block transform transition-transform duration-500 ease-in-out"
            :class="isDark ? 'rotate-0 opacity-100' : '-rotate-180 opacity-0'">
            ☀
          </span>
          <span class="inline-block transform transition-transform duration-500 ease-in-out absolute"
            :class="isDark ? 'rotate-180 opacity-0' : 'rotate-0 opacity-100'">
            🌙
          </span>
          <span class="sr-only">Toggle Dark Mode</span>
        </button>

        <!-- Mock Mode Icon -->
        <div :title="mockLabel" class="text-sm px-2 py-1 rounded-full transition"
          :class="isMock ? 'bg-green-600 text-white' : 'bg-gray-400 dark:bg-gray-600 text-white'">
          🧪 {{ isMock ? 'Mock ON' : 'Mock OFF' }}
        </div>
      </div>
    </div>
  </header>
</template>

<script setup lang="ts">
import { useDarkMode } from '@/composables/useDarkMode'
import { useRoute, useRouter } from 'vue-router'
import { computed } from 'vue'

const route = useRoute()
const router = useRouter()

// Whether user is on /dashboard
const isDashboard = computed(() => route.path === '/dashboard')

// Navigate between home and dashboard
function navigate() {
  router.push(isDashboard.value ? '/' : '/dashboard')
}


const { isDark, toggleDark } = useDarkMode()

const isMock = import.meta.env.VITE_USE_MOCK === 'true'
const mockLabel = isMock ? 'Youre using mocking mode' : 'Youre using real mode'
</script>
