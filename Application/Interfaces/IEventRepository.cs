using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces;

public interface IEventRepository
{
    Task<IEnumerable<Event>> GetAllAsync();
    Task<Event?> GetByIdAsync(Guid id);
    Task AddAsync(Event evento);
    Task UpdateAsync(Event evento);
    Task DeleteAsync(Guid id);
    //Task<IEnumerable<Joke>> GetPagedAsync(int skip, int take, string? type = null);

}
