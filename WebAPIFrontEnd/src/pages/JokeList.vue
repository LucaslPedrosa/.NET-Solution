<template>
  <div class="min-h-screen bg-white text-black dark:bg-zinc-950 dark:text-white transition-colors duration-300">
    <!-- 🔍 Filter section with mode buttons and inputs -->
    <div class="fixed top-20 left-0 w-full flex justify-center z-40 flex-wrap sm:flex-nowrap">
      <div class="rounded-lg shadow px-6 py-4 w-full max-w-5xl flex flex-col gap-2 sm:flex-row sm:items-center
      bg-gray-100 text-black dark:bg-zinc-900 dark:text-white transition-colors duration-300">
        <!-- Mode toggle buttons -->
        <div class="flex flex-wrap sm:flex-nowrap gap-2 whitespace-nowrap">
          <button @click="searchMode = 'type'"
            :class="searchMode === 'type' ? 'bg-blue-600 text-white' : 'bg-gray-200 dark:bg-zinc-700 dark:text-gray-300'"
            class="px-4 py-2 rounded transition-all duration-300 font-medium shadow text-sm hover:scale-[1.03]">
            🔤 By Type
          </button>
          <button @click="searchMode = 'externalId'"
            :class="searchMode === 'externalId' ? 'bg-blue-600 text-white' : 'bg-gray-200 dark:bg-zinc-700 dark:text-gray-300'"
            class="px-4 py-2 rounded transition-all duration-300 font-medium shadow text-sm hover:scale-[1.03]">
            🆔 By ID
          </button>
        </div>
        <!-- Input fields and search button -->
        <div class="flex-1 flex flex-col sm:flex-row gap-2 mt-2 sm:mt-0">
          <!-- Input -->
          <input v-if="searchMode === 'type'" v-model="typeFilter" type="text" placeholder="Filter by type..."
            class="flex-1 px-3 py-2 rounded bg-white text-black border border-gray-300 placeholder-gray-500
           dark:bg-zinc-800 dark:text-white dark:border-zinc-600 dark:placeholder-gray-400 transition-colors duration-300" />

          <input v-else v-model.number="externalIdInput" type="number" placeholder="Filter by External ID"
            class="flex-1 px-3 py-2 rounded bg-white text-black border border-gray-300 placeholder-gray-500
           dark:bg-zinc-800 dark:text-white dark:border-zinc-600 dark:placeholder-gray-400 transition-colors duration-300" />

          <!-- Search -->
          <div class="flex gap-2">
            <button @click="onSearch"
              class="whitespace-nowrap bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded transition">
              Search
            </button>

            <!-- 🎲 Random Joke -->
            <button @click="getRandomJoke"
              class="whitespace-nowrap bg-green-600 hover:bg-green-700 text-white px-4 py-2 rounded transition">
              🎲 Random
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- 📜 Main content area -->
    <main class="realCenter w-full">
      <div class="max-w-xl mx-auto space-y-11">
        <!-- Single joke result (search by ID) -->
        <JokeCard v-if="jokeById" :joke="jokeById" :likes="likes" @like="onLike" />

        <!-- List of jokes (search by type) -->
        <template v-if="jokeList.length && !jokeById">
          <JokeCard v-for="joke in jokeList" :key="joke.id" :joke="joke" :likes="likes" @like="onLike" />
        </template>

        <!-- Fallback: no jokes found -->
        <div v-if="!jokeById && !isLoading && jokeList.length === 0"
          class="text-center text-gray-500 dark:text-gray-400 transition-colors duration-300">
          😢 <span class="font-semibold">No jokes found</span>
          <div class="text-sm mt-1">But hey, at least the awkward silence is performing well.</div>
        </div>
      </div>
      <!-- Extra spacing so last card is not hidden behind fixed pagination -->
      <div class="pb-32"></div>
    </main>

    <!-- 📍 Pagination controls -->
    <div class="fixed bottom-4 left-0 w-full flex justify-center z-50 pointer-events-none">
      <div class="flex items-center gap-4 bg-transparent px-4 py-2 rounded pointer-events-auto">
        <button @click="goToPreviousPage" :disabled="skip === 0"
          class="min-w-[110px] text-center bg-zinc-700 hover:bg-zinc-600 text-white px-4 py-2 rounded disabled:opacity-50 disabled:cursor-not-allowed transition-colors duration-300">
          ◀ Previous
        </button>

        <span class="text-black dark:text-white font-semibold text-sm text-center">
          Page {{ currentPage }}
        </span>

        <button @click="goToNextPage"
          class="min-w-[110px] text-center bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded transition-colors duration-300">
          Next ▶
        </button>
      </div>
      <!-- simple dif for spacing -->
      <div class="h-200"></div>
    </div>

  </div>

</template>

<style>
.realCenter {
  position: absolute;
  top: 200px;
  left: 50%;
  transform: translateX(-50%);
}

div[v-for] {
  transition: transform 0.2s ease;
}

div[v-for]:hover {
  transform: translateY(-3px);
  box-shadow: 0 8px 16px rgba(0, 0, 0, 0.3);
}
</style>

<script setup lang="ts">
import { saveJoke } from '@/services/jokeService'
import { ref, computed } from 'vue'
import type { Joke } from '../services/jokeProvider'
import JokeCard from '@/components/JokeCard.vue'
import { useMockMode } from '@/composables/useMockMode'
import * as realProvider from '@/services/jokeProvider'
import * as mockProvider from '@/services/jokeMockProvider'
import axios from 'axios'

const { isMock } = useMockMode()
const provider = isMock ? mockProvider : realProvider

const jokeList = ref<Joke[]>([])
const skip = ref(0)
const take = 10
const typeFilter = ref('')
const externalIdInput = ref<number | null>(null)
const jokeById = ref<Joke | null>(null)
const searchedById = ref(false)
const currentPage = computed(() => Math.floor(skip.value / take) + 1)
const likes = ref<Record<string, number>>({})
const isLoading = ref(false)

async function onLike(jokeId: string) {
  if (isMock) {
    alert('Mock mode: Likes are not saved.')
    likes.value[jokeId] = (likes.value[jokeId] || 0) + 1
    return
  }
  try {
    const response = await fetch(`${import.meta.env.VITE_API_BASE_URL}/api/jokes/${jokeId}/like`, {
      method: 'POST',
    })

    if (!response.ok) throw new Error('Failed to like joke')

    const updatedJoke = await response.json()
    likes.value[jokeId] = updatedJoke.likes
  } catch (error) {
    console.error('Error liking joke:', error)
  }
}

async function getRandomJoke() {
  try {
    const response = await axios.get('https://official-joke-api.appspot.com/jokes/random')
    const joke = response.data

    // Build a new Joke object to match your DB format
    const newJoke = {
      id: crypto.randomUUID(), // ← optional: remove if backend generates
      externalId: joke.id,
      setup: joke.setup,
      punchline: joke.punchline,
      type: joke.type,
      createdAt: new Date().toISOString(), // ← optional: remove if backend generates
      likes: 0, // ← optional: remove if backend generates
    }

    // Save to backend
    await saveJoke(newJoke)

    // Show it in the UI
    jokeById.value = newJoke
    jokeList.value = []
    skip.value = 0
    searchMode.value = 'type'

    // Update likes state (starts at 0)
    likes.value[newJoke.id] = 0
  } catch (error) {
    console.error('Failed to fetch or save random joke:', error)
  }
}

function resetSearchState() {
  typeFilter.value = ''
  externalIdInput.value = null
  jokeById.value = null
  searchedById.value = false
  fetchJokesByType()
}

async function fetchJokesByType() {
  jokeById.value = null
  isLoading.value = true

  const result = await provider.getJokes(typeFilter.value, skip.value, take)

  if (result.length === 0 && skip.value > 0) {
    skip.value -= take
    return fetchJokesByType()
  }

  jokeList.value = result
  likes.value = Object.fromEntries(result.map(j => [j.id, j.likes]))
  isLoading.value = false
}

async function fetchJokeById() {
  searchedById.value = true
  isLoading.value = true

  if (externalIdInput.value != null) {
    const result = await provider.getJokeByExternalId(externalIdInput.value)
    jokeById.value = result

    if (result) {
      likes.value[result.id] = result.likes
    }
  } else {
    resetSearchState()
  }
  isLoading.value = false
}

function goToNextPage() {
  skip.value += take
  fetchJokesByType()
}

function goToPreviousPage() {
  if (skip.value >= take) skip.value -= take
  fetchJokesByType()
}

const searchMode = ref<'type' | 'externalId'>('type')

function onSearch() {
  skip.value = 0
  jokeList.value = []
  jokeById.value = null
  searchedById.value = false

  if (searchMode.value === 'type') {
    fetchJokesByType()
  } else {
    if (!externalIdInput.value || isNaN(externalIdInput.value)) {
      typeFilter.value = ''
      fetchJokesByType()
    } else {
      fetchJokeById()
    }
  }
}

fetchJokesByType()
</script>
