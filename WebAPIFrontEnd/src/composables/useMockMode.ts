export function useMockMode() {
  const isMock = import.meta.env.VITE_USE_MOCK === 'true'
  return { isMock }
}