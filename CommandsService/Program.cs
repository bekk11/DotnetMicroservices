using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

const string apiVersion = "v1.0";

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc(apiVersion, new OpenApiInfo { Title = "Internship Platform API", Version = apiVersion });
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UseSwagger(options => { options.RouteTemplate = "api/c/swagger/{documentName}/swagger.json"; });
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint($"{apiVersion}/swagger.json", apiVersion);
    options.RoutePrefix = "api/c/swagger";
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();