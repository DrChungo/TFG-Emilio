using GameDB.API.Config;
using GameDB.API.Infrastructure;
using GameDB.API.Repositories;
using GameDB.API.Repositories.Interfaces;
using GameDB.API.Services;
using GameDB.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

//  Configuración tipada 
builder.Services.Configure<IgdbSettings>(
    builder.Configuration.GetSection(IgdbSettings.SectionName));

builder.Services.Configure<CacheSettings>(
    builder.Configuration.GetSection(CacheSettings.SectionName));

//  Base de datos SQLite con EF Core
builder.Services.AddDbContext<GameDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// HttpClient para llamadas a IGDB
builder.Services.AddHttpClient("IgdbClient");

// CORS para el frontend React
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
// Repositorios
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();

// Servicios 
builder.Services.AddScoped<IIgdbService, IgdbService>();
builder.Services.AddScoped<IGameService, GameService>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReactApp");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();