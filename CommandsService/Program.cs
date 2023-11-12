using CommandsService.AsyncDataServices;
using CommandsService.Data;
using CommandsService.EventProcessing;
using CommandsService.SyncDataService.Grpc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

const string apiVersion = "v1.0";

// Add services to the container.
builder.Services.AddScoped<ICommandRepo, CommandRepo>();
builder.Services.AddSingleton<IEventProcessor, EventProcessor>();

builder.Services.AddControllers();

builder.Services.AddHostedService<MessageBusSubscriber>();
    
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IPlatformDataClient, PlatformDataClient>();
builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc(apiVersion, new OpenApiInfo { Title = "Internship Platform API", Version = apiVersion });
    }
);

builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("InMem"));

var app = builder.Build();

app.UseSwagger(options => { options.RouteTemplate = "api/c/swagger/{documentName}/swagger.json"; });
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint($"{apiVersion}/swagger.json", apiVersion);
    options.RoutePrefix = "api/c/swagger";
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

PrepDb.PrepPopulation(app);

app.Run();