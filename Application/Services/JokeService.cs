using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions;
using Domain.Entities;
using Shared.DTOs;
using Application.Interfaces;
using Application.Mappings;


namespace Application.Services;

public class JokeService : IJokeService
{
    private readonly IJokeRepository _repository;

    public JokeService(IJokeRepository repository)
    {
        _repository = repository;
    }

    public async Task<JokeDto> GetByIdAsync(Guid id)
    {
        var joke = await _repository.GetByIdAsync(id);

        if (joke == null)
            throw new NotFoundException("We searched high and low, but the joke packed its bags and left town.");

        return joke.ToDto();
    }

    public async Task DeleteAsync(Guid id)
    {
        var joke = await _repository.GetByIdAsync(id);

        if (joke == null)
            throw new NotFoundException("Tried to delete a joke that ghosted us—classic.");

        if (joke.Setup.Length < 10)
            throw new ValidationException("This joke was so short, even a tweet felt verbose in comparison. Not worth deleting.");

        await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<JokeDto>> GetPagedAsync(int skip, int take, string? type)
    {
        if (take > 100)
            throw new ValidationException("100 jokes max—any more might collapse the humor dimension.");

        var jokes = await _repository.GetPagedAsync(skip, take, type);

        return jokes.ToDtoList();
    }

    public async Task<JokeDto> GetByExternalIdAsync(int externalId)
    {
        var joke = await _repository.GetByExternalIdAsync(externalId);

        if (joke == null)
            throw new NotFoundException($"Looks like that joke took its ID and entered witness protection. {externalId}");

        return joke.ToDto();
    }

    public async Task<object> GetStatsAsync()
    {
        var total = await _repository.CountAsync();
        var mostLiked = await _repository.GetMostLikedAsync();
        var grouped = await _repository.GetGroupedByTypeAsync();
        var totalLikes = await _repository.GetLikesSum();

        return new
        {
            TotalJokes = total,
            MostLiked = mostLiked,
            JokesByType = grouped,
            TotalLikes = totalLikes
        };
    }

    public async Task UpdateAsync(JokeDto jokeDto)
    {
        var joke = jokeDto.ToEntity();
        await _repository.UpdateAsync(joke);
    }
    public async Task UpsertAsync(JokeDto jokeDto)
    {
        var joke = jokeDto.ToEntity();
        await _repository.UpsertAsync(joke);
    }

}
