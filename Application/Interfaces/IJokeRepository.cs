using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Shared.DTOs;

namespace Application.Interfaces;

public interface IJokeRepository
{
    Task<IEnumerable<Joke>> GetAllAsync();
    Task AddAsync(Joke joke);
    Task<Joke?> GetByIdAsync(Guid id);              
    Task<IEnumerable<Joke>> GetByTypeAsync(string type);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<Joke>> GetPagedAsync(int skip, int take, string? type = null);
    Task UpsertAsync(Joke joke);
    Task<Joke?> GetByExternalIdAsync(int externalId);
    IQueryable<Joke> Query();
    Task<int> CountAsync();
    Task<Joke?> GetMostLikedAsync();
    Task<List<JokeTypeCountDto>> GetGroupedByTypeAsync();
    Task<int> GetLikesSum();
    Task UpdateAsync(Joke joke);
}