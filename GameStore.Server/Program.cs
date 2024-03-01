using GameStore.Server.Data;
using GameStore.Server.Models;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => options.AddDefaultPolicy(builder =>
{
    builder.WithOrigins("http://localhost:5290")
        .AllowAnyHeader()
        .AllowAnyMethod();
}));

var connString = builder.Configuration.GetConnectionString("GameStoreContext");
builder.Services.AddSqlServer<GameStoreContext>(connString);

var app = builder.Build();

app.UseCors();

// grouping endpoints with similar urls
var group = app.MapGroup("/games")
    // Validation for Game method from MinimalApis.Extensions package
    .WithParameterValidation();


// GET /games
group.MapGet("/", async (GameStoreContext context) =>
    await context.Games.AsNoTracking().ToListAsync());

// GET /games/{id}
group.MapGet("/{id}", async (int id, GameStoreContext context) =>
{
    Game? game = await context.Games.FindAsync(id);

    if (game == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(game);
})
.WithName("GetGame");

// POST /games
group.MapPost("/", async (Game game, GameStoreContext context) =>
{
    context.Games.Add(game);
    await context.SaveChangesAsync();

    return Results.CreatedAtRoute("GetGame", new { id = game.Id }, game);
});

// PUT /games/{id}
group.MapPut("/{id}", async (int id, Game updatedGame, GameStoreContext context) =>
{
    var rowsAffected = await context.Games.Where(game => game.Id == id)
        .ExecuteUpdateAsync(updates =>
            updates.SetProperty(game => game.Name, updatedGame.Name)
                .SetProperty(game => game.Genre, updatedGame.Genre)
                .SetProperty(game => game.Price, updatedGame.Price)
                .SetProperty(game => game.ReleaseDate, updatedGame.ReleaseDate));
    
    return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
});

// DELETE /games/{id}
group.MapDelete("/{id}", async (int id, GameStoreContext context) =>
{
    var rowsAffected = await context.Games.Where(game => game.Id == id)
        .ExecuteDeleteAsync();

    return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
});

app.Run();
