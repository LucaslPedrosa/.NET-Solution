using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Newtonsoft.Json;
using Shared.DTOs;
using Application.Mappings;

namespace WorkerService.Services;

public class JokeService
{
    private readonly IJokeRepository _jokeRepository;
    private readonly HttpClient _httpClient;

    public JokeService(IJokeRepository jokeRepository, HttpClient httpClient)
    {
        _jokeRepository = jokeRepository;
        _httpClient = httpClient;
    }

    public async Task FetchAndStoreJokeAsync()
    {
        try
        {
            var response = await _httpClient.GetStringAsync("https://official-joke-api.appspot.com/random_joke");
            var fetchedJoke = JsonConvert.DeserializeObject<JokeDto>(response);

            if (fetchedJoke != null)
            {
                await _jokeRepository.UpsertAsync(fetchedJoke.ToEntity());
                Console.WriteLine($"[Hangfire] Saved Joke: {fetchedJoke.Setup} - {fetchedJoke.Punchline}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Hangfire] Error fetching or saving joke: {ex.Message}");
        }
    }
}