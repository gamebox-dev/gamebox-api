using GameBox.Connectors;
using GameBox.Connectors.IGDB;
using GameBox.Controllers;
using GameBox.Utilities;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var clientID = EnvironmentUtility.GetVariable("IGDB_CLIENT_ID");
var clientSecret = EnvironmentUtility.GetVariable("IGDB_CLIENT_SECRET");
builder.Services.AddSingleton<IGameSource>(s => new IGDBGameSource(clientID, clientSecret));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "GameBox API",
            Version = "V0.0.1"
        }
    );
    c.IncludeXmlComments(typeof(HelloController).Assembly);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
