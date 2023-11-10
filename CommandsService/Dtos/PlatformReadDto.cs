namespace CommandsService.Dtos;

public class PlatformReadDto
{
    public PlatformReadDto(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; set; }
    public string Name { get; set; }
}