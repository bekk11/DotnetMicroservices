using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CommandsService.Models;

public class Command
{
    [Key] [Required] public int Id { get; set; }
    [Required] public string HowTo { get; set; }
    [Required] public string CommandLine { get; set; }
    [Required] public int PlatformId { get; set; }
    [JsonIgnore] public Platform Platform { get; set; }
}