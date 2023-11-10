using System.Text.Json;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;

namespace CommandsService.EventProcessing;

public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMapper _mapper;

    public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
    {
        _scopeFactory = scopeFactory;
        _mapper = mapper;
    }
    
    public void ProcessEvent(string message)
    {
        EventType eventType = DetermineEvent(message);

        switch (eventType)
        {
            case EventType.PlatformPublished:
                AddPlatform(message);
                break;
            default:
                Console.WriteLine("--> Undetected event type!");
                break;
        }
    }

    private EventType DetermineEvent(string notificationMessage)
    {
        Console.WriteLine("--> Determining Event");

        GenericEventDto? eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

        switch (eventType?.Event)
        {
            case "Platform_Published":
                Console.WriteLine("--> Platform published event detected");
                return EventType.PlatformPublished;
            default:
                Console.WriteLine("--> Could not determine the event type");
                return EventType.Undetermined;
        }
    }

    private void AddPlatform(string platformPublishedMessage)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        ICommandRepo repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();
        
        PlatformPublishDto platformPublishDto = JsonSerializer.Deserialize<PlatformPublishDto>(platformPublishedMessage) ?? throw new InvalidOperationException();

        try
        {
            Platform platform = _mapper.Map<Platform>(platformPublishDto);
            if (repo.ExternalPlatformExists(platform.ExternalId))
            {
                Console.WriteLine("--> Platform already exists...");
                return;
            }
            
            repo.CreatePlatform(platform);
            repo.SaveChanges();
            Console.WriteLine("--> Platform Added");
        }
        catch (Exception e)
        {
            Console.WriteLine($"--> Could not add Platform to DB {e.Message}");
        }
    }
}

enum EventType
{
    PlatformPublished,
    Undetermined
}