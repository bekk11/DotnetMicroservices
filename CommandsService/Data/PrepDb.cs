using System.Collections;
using CommandsService.Models;
using CommandsService.SyncDataService.Grpc;

namespace CommandsService.Data;

public class PrepDb
{
    public static void PrepPopulation(IApplicationBuilder applicationBuilder)
    {
        using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();

        IPlatformDataClient grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>() ?? throw new InvalidOperationException();

        IEnumerable<Platform> platforms = grpcClient.ReturnAllPlatforms();
        
        SeedData(serviceScope.ServiceProvider.GetService<ICommandRepo>() ?? throw new InvalidOperationException(), platforms);
    }

    private static void SeedData(ICommandRepo repo, IEnumerable<Platform> platforms)
    {
        Console.WriteLine("Seeding new platforms...");

        foreach (var platform in platforms)
        {
            if (!repo.PlatformExists(platform.ExternalId))
            {
                repo.CreatePlatform(platform);
            }

            repo.SaveChanges();
        }
    }
}