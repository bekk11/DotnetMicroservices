using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data;

public static class PrepDb
{
    public static void PrepPopulation(IApplicationBuilder applicationBuilder, bool isProduction)
    {
        using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();
        SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>() ?? throw new InvalidOperationException(), isProduction);
    }

    private static void SeedData(AppDbContext context, bool isProduction)
    {
        if (isProduction)
        {
            Console.WriteLine("--> Attempting to apply migrations...");
            try
            {
                context.Database.Migrate();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not run migrations: {e.Message}");
                throw;
            }
        }

        if (!context.Platforms.Any())
        {
            Console.WriteLine("--> Seeding Data...");

            context.Platforms.AddRange(
                new Platform("Dotnet", "Microsoft", "Free"),
                new Platform("Sql Sever Express", "Microsoft", "Free"),
                new Platform("Kubernetes", "Cloud Native Computing Foundation", "Free")
            );

            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("--> We already have data");
        }
    }
}