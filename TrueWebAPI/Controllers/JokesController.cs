using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;


namespace TrueWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JokesController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly IJokeService _jokeService;


    public JokesController(HttpClient httpClient, IJokeService jokeService)
    {
        _httpClient = httpClient;
        _jokeService = jokeService;
    }

    // 🔹 Search on external API
    [HttpGet("live")]
    public async Task<IActionResult> GetLiveJoke()
    {
        var response = await _httpClient.GetAsync("https://official-joke-api.appspot.com/random_joke");

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, "Error fetching joke from API.");
        }

        var json = await response.Content.ReadAsStringAsync();
        var joke = JsonSerializer.Deserialize<JokeDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (joke == null)
        {
            return BadRequest("Failed to parse joke.");
        }

        return Ok(joke);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var joke = await _jokeService.GetByIdAsync(id); // ← Service validation
        return Ok(joke);
    }

    [HttpGet] // 🔹 Jokes on local database by pagination
    public async Task<IActionResult> GetPaged(
    [FromQuery] string? type,
    [FromQuery] int skip = 0,
    [FromQuery] int take = 10)
    {
        var jokes = await _jokeService.GetPagedAsync(skip, take, type);
        return Ok(jokes);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _jokeService.DeleteAsync(id); // ← Validation in service
        return NoContent();
    }

    [HttpGet("external/{externalId:int}")]
    public async Task<IActionResult> GetByExternalId(int externalId)
    {
        var joke = await _jokeService.GetByExternalIdAsync(externalId);

        if (joke == null)
            return NotFound(new { message = $"Joke with externalId {externalId} not found." });

        return Ok(joke);
    }


    [HttpGet("stats")]  
    public async Task<IActionResult> GetStats()
    {
        var stats = await _jokeService.GetStatsAsync();
        return Ok(stats);
    }

    [HttpPost("{id:guid}/like")]
    public async Task<IActionResult> Like(Guid id)
    {
        var joke = await _jokeService.GetByIdAsync(id);
        if (joke == null) return NotFound();

        joke.Likes++;
        await _jokeService.UpdateAsync(joke);

        return Ok(joke);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrUpdate([FromBody] JokeDto jokeDto)
    {
        await _jokeService.UpsertAsync(jokeDto);
        return Ok();
    }
}
