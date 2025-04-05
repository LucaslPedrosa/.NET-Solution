using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IJokeService
    {
        Task<JokeDto> GetByIdAsync(Guid id);
        Task<IEnumerable<JokeDto>> GetPagedAsync(int skip, int take, string? type);
        Task<JokeDto> GetByExternalIdAsync(int externalId);
        Task DeleteAsync(Guid id);
        Task UpdateAsync(JokeDto jokeDto);
        Task UpsertAsync(JokeDto jokeDto);
        Task<object> GetStatsAsync();
    }
}
