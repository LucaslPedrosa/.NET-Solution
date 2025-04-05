using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;
public class Joke
{
    public Guid Id { get; set; }
    public string Setup { get; set; } = string.Empty;
    public string Punchline { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Type { get; set; } = string.Empty;
    public int ExternalId { get; set; }
    public int Likes { get; set; } = 0;

}