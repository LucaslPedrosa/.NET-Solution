import axios from 'axios'
import type { Joke } from '@/models/Joke'


const api = axios.create({
  baseURL: 'http://localhost:5197/api',
})

export interface StatsResponse {
  totalJokes: number
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

export async function saveJoke(joke: Joke): Promise<void> {
  await api.post('/jokes', joke)
}

export async function getStats(): Promise<StatsResponse> {
  const response = await api.get<StatsResponse>('/jokes/stats')
  return response.data
}

// 🔹 Search more than one joke
export async function getJokes(type?: string, skip = 0, take = 10): Promise<Joke[]> {
  const params: any = { skip, take }
  if (type) params.type = type

  const response = await api.get<Joke[]>('/jokes', { params })
  return response.data
}

// 🔹 Search joke by externalID
export async function getJokeByExternalId(externalId: number): Promise<Joke | null> {
  try {
    const response = await api.get<Joke>(`/jokes/external/${externalId}`)
    return response.data
  } catch (error) {
    console.error('Joke not found or error occurred:', error)
    return null
  }
}
