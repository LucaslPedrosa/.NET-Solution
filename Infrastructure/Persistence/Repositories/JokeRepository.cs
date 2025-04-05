using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;

namespace Infrastructure.Persistence.Repositories;

public class JokeRepository : IJokeRepository
{
    private readonly ApplicationDbContext _context;

    public JokeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Joke>> GetAllAsync()
    {
        return await _context.Set<Joke>().OrderByDescending(j => j.CreatedAt).ToListAsync();

    }

    public async Task AddAsync(Joke joke)
    {
        _context.Set<Joke>().Add(joke);
        await _context.SaveChangesAsync();
    }

     public async Task<Joke?> GetByIdAsync(Guid id)
    {
     return await _context.Jokes.FindAsync(id);
    }

    public async Task<IEnumerable<Joke>> GetByTypeAsync(string type)
    {
        return await _context.Jokes
            .Where(j => j.Type.ToLower().Equals(type, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(j => j.CreatedAt)
            .ToListAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var joke = await _context.Jokes.FindAsync(id);
        if (joke is not null)
        {
            _context.Jokes.Remove(joke);
            await _context.SaveChangesAsync();
        }
}

    public async Task<IEnumerable<Joke>> GetPagedAsync(int skip, int take, string? type = null)
    {
        var query = _context.Jokes.AsQueryable();

        if (!string.IsNullOrWhiteSpace(type))
            query = query.Where(j => j.Type.ToLower() == type.ToLower());

        return await query
            .OrderByDescending(j => j.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task UpsertAsync(Joke joke)
    {
        var existing = await _context.Jokes.FirstOrDefaultAsync(j => j.Setup == joke.Setup && j.Punchline == joke.Punchline);

        if (existing != null)
        {
            existing.Type = joke.Type;
            existing.CreatedAt = joke.CreatedAt;
        }
        else
        {
            await _context.Jokes.AddAsync(joke);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<Joke?> GetByExternalIdAsync(int externalId)
    {
        return await _context.Jokes
            .FirstOrDefaultAsync(j => j.ExternalId == externalId);
    }

    public IQueryable<Joke> Query() => _context.Jokes.AsQueryable();

    public async Task<int> CountAsync()
    {
        return await _context.Jokes.CountAsync();
    }

    public async Task<Joke?> GetMostLikedAsync()
    {
        return await _context.Jokes
            .OrderByDescending(j => j.Likes)
            .FirstOrDefaultAsync();
    }

    public async Task<List<JokeTypeCountDto>> GetGroupedByTypeAsync()
    {
        return await _context.Jokes
            .GroupBy(j => j.Type)
            .Select(g => new JokeTypeCountDto
            {
                Type = g.Key,
                Count = g.Count()
            })
            .ToListAsync();
    }

    public async Task<int> GetLikesSum()
    {
        return await _context.Jokes.SumAsync(j => j.Likes);
    }

    public async Task UpdateAsync(Joke joke)
    {
        var local = _context.Jokes.Local.FirstOrDefault(e => e.Id == joke.Id);

        if (local != null)
        {
            _context.Entry(local).State = EntityState.Detached;
        }

        _context.Entry(joke).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
}