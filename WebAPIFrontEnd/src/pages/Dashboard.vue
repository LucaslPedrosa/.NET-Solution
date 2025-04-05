<template>
  <div class="realCenter">
    <div
      class="min-h-screen px-6 py-20 bg-white text-black dark:bg-zinc-950 dark:text-white transition-colors duration-300">
      <div class="max-w-4xl mx-auto text-center">
        <h1 class="text-4xl font-bold mb-4">📊 Stats Dashboard</h1>
        <p class="text-gray-600 dark:text-gray-300 mb-10">
          Insights and metrics from your joke database.
        </p>

        <!-- Stats Cards -->

        <!-- Total Jokes -->
        <div class="grid grid-cols-1 sm:grid-cols-2 gap-6">
          <div class="bg-gray-100 dark:bg-zinc-800 rounded-lg shadow p-6">
            <h2 class="text-xl font-semibold mb-2">Total Jokes</h2>
            <p class="text-3xl font-bold">{{ stats?.totalJokes ?? '...' }}</p>
          </div>

          <!-- Most Liked Joke -->
          <div class="bg-gray-100 dark:bg-zinc-800 rounded-lg shadow p-6">
            <h2 class="text-xl font-semibold mb-2">Most Liked Type</h2>
            <p class="text-3xl font-bold">
              {{
                stats?.jokesByType.reduce((a, b) => (a.count > b.count ? a : b), { type: '...', count: 0 }).type ?? '...'
              }}
            </p>
          </div>

          <!-- Top Joke -->
          <div class="bg-gray-100 dark:bg-zinc-800 rounded-lg shadow p-6">
            <h2 class="text-xl font-semibold mb-2">Top Joke</h2>
            <p class="text-3xl font-bold">
              {{ stats?.mostLiked?.setup ?? '...' }}
            </p>
          </div>

          <!-- Total Likes -->
          <div class="bg-gray-100 dark:bg-zinc-800 rounded-lg shadow p-6">
            <h2 class="text-xl font-semibold mb-2">Total Likes</h2>
            <p class="text-3xl font-bold">
              {{stats?.totalLikes ?? '...'}}
            </p>
          </div>
        </div>

        <!-- Jokes by Type Chart Placeholder -->
        <div class="mt-12">
          <h2 class="text-2xl font-bold mb-4">Jokes by Type</h2>
          <div class="bg-gray-100 dark:bg-zinc-800 rounded-lg shadow p-6">
            <ul class="text-left space-y-1">
              <li v-for="t in stats?.jokesByType ?? []" :key="t.type" class="text-sm">
                • <strong>{{ t.type }}</strong>: {{ t.count }}
              </li>
            </ul>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue'
import axios from 'axios'

const isMock = import.meta.env.VITE_USE_MOCK === 'true'


interface StatsResponse {
  totalJokes: number
  totalLikes: number
  mostLiked: {
    id: string
    setup: string
    punchline: string
    likes: number
  } | null
  jokesByType: {
    type: string
    count: number
  }[]
}

const stats = ref<StatsResponse | null>(null)

onMounted(async () => {
  try {

    if (isMock) {
      // Mocked stats
      stats.value = {
        totalJokes: 15,
        totalLikes: 1000,
        mostLiked: {
          id: '101',
          setup: 'Why did the duck cross the road?',
          punchline: "To say 'quack quack' to the other side!",
          likes: 999,
        },
        jokesByType: [
          { type: 'general', count: 2 },
          { type: 'animal', count: 3 },
          { type: 'nerd', count: 1 },
          {type: 'spooky', count: 2},
          { type: 'food', count: 3 },
          { type: 'tech', count: 2 },
          { type: 'school', count: 1 },
          { type: 'pirate', count: 1 },
        ]
      }


    } else {
      const response = await axios.get(`${import.meta.env.VITE_API_BASE_URL}/api/jokes/stats`)
      console.log("📦 Stats response:", response.data)
      stats.value = response.data
    }


  } catch (error) {
    console.error('Error loading stats:', error)
  }
})
</script>

<style scoped>
.realCenter {
  position: absolute;
  top: 200px;
  left: 50%;
  transform: translateX(-50%);
}
</style>
