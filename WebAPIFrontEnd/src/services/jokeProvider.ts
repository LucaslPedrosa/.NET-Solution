import * as RealService from './jokeService'
import * as MockService from './jokeMockProvider'

const useMock = import.meta.env.VITE_USE_MOCK === 'true'

export const getJokes = useMock ? MockService.getJokes : RealService.getJokes
export const getJokeByExternalId = useMock
  ? MockService.getJokeByExternalId
  : RealService.getJokeByExternalId

export type { Joke } from '@/models/Joke'
