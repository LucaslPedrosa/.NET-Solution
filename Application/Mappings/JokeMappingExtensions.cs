using Domain.Entities;
using Shared.DTOs;

namespace Application.Mappings;

public static class JokeMappingExtensions
{
    public static JokeDto ToDto(this Joke joke)
    {
        return new JokeDto
        {
            Id = joke.Id,
            ExternalId = joke.ExternalId,
            Type = joke.Type,
            Setup = joke.Setup,
            Punchline = joke.Punchline,
            CreatedAt = joke.CreatedAt,
            Likes = joke.Likes
        };
    }

    public static IEnumerable<JokeDto> ToDtoList(this IEnumerable<Joke> jokes)
    {
        return jokes.Select(j => j.ToDto());
    }

    public static Joke ToEntity(this JokeDto dto)
    {
        return new Joke
        {
            Id = dto.Id,
            ExternalId = dto.ExternalId,
            Type = dto.Type,
            Setup = dto.Setup,
            Punchline = dto.Punchline,
            CreatedAt = dto.CreatedAt,
            Likes = dto.Likes
        };
    }
}
