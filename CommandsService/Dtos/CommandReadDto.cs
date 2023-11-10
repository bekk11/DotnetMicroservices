namespace CommandsService.Dtos;

public class CommandReadDto
{
    public CommandReadDto(int id, string howTo, string commandLine, int platformId)
    {
        Id = id;
        HowTo = howTo;
        CommandLine = commandLine;
        PlatformId = platformId;
    }

    public int Id { get; set; }
    public string HowTo { get; set; }
    public string CommandLine { get; set; }
    public int PlatformId { get; set; }
}