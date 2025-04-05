import type { Joke } from "@/models/Joke";

const fakeJokes: Joke[] = [
  {
    id: "1",
    externalId: 101,
    setup: "Why did the duck cross the road?",
    punchline: "To say 'quack quack' to the other side!",
    type: "animal",
    createdAt: new Date().toISOString(),
    likes: 0,
  },
  {
    id: "2",
    externalId: 102,
    setup: "How does an electron answer the phone?",
    punchline: "Proton day!",
    type: "nerd",
    createdAt: new Date().toISOString(),
    likes: 0,
  },
  {
    id: "3",
    externalId: 103,
    setup: "What’s the ultimate speed?",
    punchline: "Going around the block and passing yourself.",
    type: "general",
    createdAt: new Date().toISOString(),
    likes: 0,
  },
  {
    id: "4",
    externalId: 104,
    setup: "Why don’t skeletons fight each other?",
    punchline: "They don’t have the guts!",
    type: "spooky",
    createdAt: new Date().toISOString(),
    likes: 0,
  },
  {
    id: "5",
    externalId: 105,
    setup: "What do you call fake spaghetti?",
    punchline: "An impasta!",
    type: "food",
    createdAt: new Date().toISOString(),
    likes: 0,
  },
  {
    id: "6",
    externalId: 106,
    setup: "Why don’t programmers prefer dark mode?",
    punchline: "Because the light attracts bugs.",
    type: "tech",
    createdAt: new Date().toISOString(),
    likes: 0,
  },
  {
    id: "7",
    externalId: 107,
    setup: "What has 4 legs and 1 arm?",
    punchline: "A pitbull coming back from the park!",
    type: "animal",
    createdAt: new Date().toISOString(),
    likes: 0,
  },
  {
    id: "8",
    externalId: 108,
    setup: "Why was the math book sad?",
    punchline: "Because it had too many problems.",
    type: "school",
    createdAt: new Date().toISOString(),
    likes: 0,
  },
  {
    id: "9",
    externalId: 109,
    setup: "What’s orange and sounds like a parrot?",
    punchline: "A carrot!",
    type: "food",
    createdAt: new Date().toISOString(),
    likes: 0,
  },
  {
    id: "10",
    externalId: 110,
    setup: "Why don’t eggs tell jokes?",
    punchline: "They’d crack up!",
    type: "food",
    createdAt: new Date().toISOString(),
    likes: 0,
  },
  {
    id: "11",
    externalId: 111,
    setup: "What do you call a bear with no teeth?",
    punchline: "A gummy bear!",
    type: "animal",
    createdAt: new Date().toISOString(),
    likes: 0,
  },
  {
    id: "12",
    externalId: 112,
    setup: "Why was the computer cold?",
    punchline: "It left its Windows open!",
    type: "tech",
    createdAt: new Date().toISOString(),
    likes: 0,
  },
  {
    id: "13",
    externalId: 113,
    setup: "What did one wall say to the other wall?",
    punchline: "I’ll meet you at the corner!",
    type: "general",
    createdAt: new Date().toISOString(),
    likes: 0,
  },
  {
    id: "14",
    externalId: 114,
    setup: "Why don’t ghosts lie?",
    punchline: "Because you can see right through them!",
    type: "spooky",
    createdAt: new Date().toISOString(),
    likes: 0,
  },
  {
    id: "15",
    externalId: 115,
    setup: "What’s a pirate’s favorite letter?",
    punchline: "You’d think it’s R, but it’s the C!",
    type: "pirate",
    createdAt: new Date().toISOString(),
    likes: 0,
  },
];

// 🔹
export async function getJokes(
  type?: string,
  skip = 0,
  take = 10
): Promise<Joke[]> {
  let result = [...fakeJokes];

  if (type) {
    result = result.filter((j) =>
      j.type.toLowerCase().includes(type.toLowerCase())
    );
  }

  return Promise.resolve(result.slice(skip, skip + take));
}

// 🔹
export async function getJokeByExternalId(
  externalId: number
): Promise<Joke | null> {
  const joke = fakeJokes.find((j) => j.externalId === externalId);
  return Promise.resolve(joke ?? null);
}
