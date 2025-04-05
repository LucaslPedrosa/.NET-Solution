using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class EventRepository : IEventRepository
{
    private readonly ApplicationDbContext _context;

    public EventRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Event>> GetAllAsync()
    {
        return await _context.Set<Event>().ToListAsync();
    }

    public async Task<Event?> GetByIdAsync(Guid id)
    {
        return await _context.Set<Event>().FindAsync(id);
    }

    public async Task AddAsync(Event evento)
    {
        _context.Set<Event>().Add(evento);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Event evento)
    {
        _context.Set<Event>().Update(evento);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var evento = await _context.Set<Event>().FindAsync(id);
        if (evento != null)
        {
            _context.Set<Event>().Remove(evento);
            await _context.SaveChangesAsync();
        }
    }
}
