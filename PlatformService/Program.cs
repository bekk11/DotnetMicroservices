using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.SyncDataServices.Grpc;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

const string apiVersion = "v1.0";

// Add services to the container.
builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
builder.Services.AddGrpc();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc(apiVersion, new OpenApiInfo { Title = "Internship Platform API", Version = apiVersion });
    }
);

if (builder.Environment.IsProduction())
{
    Console.WriteLine("--> Using SQL Server DB");
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConn")));
}
else
{
    Console.WriteLine("--> Using InMem DB");
    builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("in_mem"));
}

Console.WriteLine($"--> CommandService Endpoint {builder.Configuration["CommandService"]}");

var app = builder.Build();

app.UseSwagger(options => { options.RouteTemplate = "api/swagger/{documentName}/swagger.json"; });
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint($"{apiVersion}/swagger.json", apiVersion);
    options.RoutePrefix = "api/swagger";
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<GrpcPlatformService>();
app.MapGet("/protos/platforms.proto", async context =>
    {
        await context.Response.WriteAsync(File.ReadAllText("Protos/platforms.proto"));
    }
);

PrepDb.PrepPopulation(app, builder.Environment.IsProduction());

app.Run();