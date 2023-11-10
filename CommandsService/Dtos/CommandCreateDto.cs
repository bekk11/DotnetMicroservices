using System.ComponentModel.DataAnnotations;

namespace CommandsService.Dtos;

public class CommandCreateDto
{
    public CommandCreateDto(string howTo, string commandLine)
    {
        HowTo = howTo;
        CommandLine = commandLine;
    }

    [Required] public string HowTo { get; set; }
    [Required] public string CommandLine { get; set; }
}