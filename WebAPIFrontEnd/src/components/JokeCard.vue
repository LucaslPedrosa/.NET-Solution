<template>
  <div class="p-6 rounded-2xl shadow border bg-white text-black border-gray-300
           dark:bg-zinc-800 dark:text-white dark:border-zinc-700 hover:border-blue-500 transition-all duration-300">

    <!-- External ID -->
    <p class="text-sm text-gray-500 dark:text-gray-400 mb-2">🔗 External ID: {{ joke.externalId }}</p>

    <!-- Setup -->
    <h3 class="text-xl font-semibold mb-2">{{ joke.setup }}</h3>

    <!-- Punchline -->
    <p class="text-gray-700 dark:text-gray-300 mb-4">{{ joke.punchline }}</p>

    <!-- Type badge -->
    <div class="flex items-center justify-between text-sm text-gray-500 dark:text-gray-400 mb-2">
      <span class="inline-block bg-blue-200 text-blue-800 px-3 py-1 rounded-full text-xs font-medium capitalize">
        {{ joke.type }}
      </span>
    </div>

    <!-- Like and Date -->
    <div class="flex justify-between items-center mt-4">
      <!-- Like Button -->
      <div>
        <!-- Flying emoji animation -->
        <span ref="likeIconRef" v-show="isAnimating"
          class="absolute left-1/2 transform -translate-x-1/2 text-xl z-10 animate-fly">
          🔥
        </span>

        <!-- Like button compact version -->
        <button @click="() => { $emit('like', joke.id); animateLike() }"
          class="inline-flex items-center px-3 py-1.5 text-xs font-medium text-white bg-blue-600 rounded-full hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-400 dark:bg-blue-500 dark:hover:bg-blue-600 dark:focus:ring-blue-700 transition">
          👍 Likes
          <span
            class="ml-2 inline-flex items-center justify-center w-5 h-5 text-[10px] font-bold text-blue-800 bg-blue-200 rounded-full">
            {{ likes[joke.id] || 0 }}
          </span>
        </button>
      </div>

      <!-- Date -->
      <span class="text-xs text-gray-400 dark:text-gray-500">
        {{ new Date(joke.createdAt).toLocaleDateString() }}
      </span>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'

const props = defineProps<{
  joke: any
  likes: Record<string, number>
}>()

const emit = defineEmits(['like'])

const isBouncing = ref(false)

function handleClick() {
  emit('like', props.joke.id)
  isBouncing.value = true
  setTimeout(() => (isBouncing.value = false), 300)
}

const isAnimating = ref(false)
const likeIconRef = ref<HTMLElement | null>(null)

function animateLike() {
  if (!likeIconRef.value) return
  isAnimating.value = true

  // Reset animation after it runs
  setTimeout(() => {
    isAnimating.value = false
  }, 600)
}


</script>


<style scoped>
@keyframes bounce-short {

  0%,
  100% {
    transform: translateY(0);
  }

  50% {
    transform: translateY(-5px);
  }
}

.animate-bounce-short {
  animation: bounce-short 0.3s ease;
}

@keyframes fly-up {
  0% {
    transform: translate(-50%, 0) scale(1);
    opacity: 1;
  }

  30% {
    transform: translate(-50%, -20px) scale(1.4);
    opacity: 1;
  }

  100% {
    transform: translate(-50%, -60px) scale(0.8);
    opacity: 0;
  }
}

.animate-fly {
  animation: fly-up 0.6s ease-out;
  pointer-events: none;
}
</style>
