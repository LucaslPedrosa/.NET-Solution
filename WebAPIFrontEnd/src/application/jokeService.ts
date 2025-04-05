import { Joke } from '@/domain/joke'
import { getJokes, getJokeByExternalId } from '@/services/jokeProvider'

export async function fetchJokesByType(type: string, skip: number, take: number): Promise<Joke[]> {
  return await getJokes(type, skip, take)
}

export async function fetchJokeById(id: number): Promise<Joke | null> {
  return await getJokeByExternalId(id)
}