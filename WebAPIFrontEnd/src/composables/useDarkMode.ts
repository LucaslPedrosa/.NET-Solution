import { ref, onMounted } from 'vue'

const isDark = ref(false)

export function useDarkMode() {
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

  return { isDark, toggleDark }
}