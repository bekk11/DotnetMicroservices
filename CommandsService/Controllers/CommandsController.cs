using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers;

[ApiController]
[Route("api/c/platforms/{platformId}/[controller]")]
public class CommandsController : ControllerBase
{
    private readonly ICommandRepo _commandRepo;
    private readonly IMapper _mapper;

    public CommandsController(ICommandRepo dbContext, IMapper mapper)
    {
        _commandRepo = dbContext;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
    {
        Console.WriteLine($"--> Hit GetCommandsForPlatform: {platformId}");

        if (!_commandRepo.PlatformExists(platformId)) return NotFound();

        var commands = _commandRepo.GetCommandsForPlatform(platformId);

        return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
    }

    [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
    public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
    {
        Console.WriteLine($"--> Hit GetCommandsForPlatform: {platformId} / {commandId}");

        if (!_commandRepo.PlatformExists(platformId)) return NotFound();

        var command = _commandRepo.GetCommand(platformId, commandId);

        if (command == null) return NotFound();

        return Ok(_mapper.Map<PlatformReadDto>(command));
    }

    [HttpPost]
    public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto commandCreateDto)
    {
        Console.WriteLine($"--> Hit CreateCommandForPlatform: {platformId}");

        if (!_commandRepo.PlatformExists(platformId)) return NotFound();

        var command = _mapper.Map<Command>(commandCreateDto);
        _commandRepo.CreateCommand(platformId, command);
        _commandRepo.SaveChanges();

        var commandReadDto = _mapper.Map<CommandReadDto>(command);

        return CreatedAtRoute(
            nameof(GetCommandForPlatform),
            new
            {
                platformId = platformId,
                commandId = commandReadDto.Id
            },
            commandReadDto
        );
    }
}