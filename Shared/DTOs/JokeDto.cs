using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Shared.DTOs;

public class JokeDto
{
    public Guid Id { get; set; } // (PK)

    [JsonProperty("id")]
    public int ExternalId { get; set; } // ID of the joke from the external API
    public string Type { get; set; } = string.Empty;
    public string Setup { get; set; } = string.Empty;
    public string Punchline { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int Likes { get; set; } = 0;
}