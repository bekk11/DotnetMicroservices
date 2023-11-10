using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CommandsService.Models;

public class Platform
{
    [Key] [Required] public int Id { get; set; }
    [Required] public int ExternalId { get; set; }
    [Required] public string Name { get; set; }

    [JsonIgnore] public ICollection<Command> Commands { get; set; } = new List<Command>();
}