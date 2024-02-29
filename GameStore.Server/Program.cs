using GameStore.Server.Data;
using GameStore.Server.Models;

List<Game> games = new()
{
    new Game()
    {
        Id = 1,
        Name = "Street Fighter II",
        Genre = "Fighting",
        Price = 19.99M,
        ReleaseDate = new DateTime(1991, 2, 1)
    },
    new Game()
    {
        Id = 2,
        Name = "Final Fantasy XIV",
        Genre = "Roleplaying",
        Price = 59.99M,
        ReleaseDate = new DateTime(2010, 9, 30)
    },
    new Game()
    {
        Id = 3,
        Name = "FIFA 23",
        Genre = "Sports",
        Price = 69.99M,
        ReleaseDate = new DateTime(2022, 9, 27)
    }
};

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
group.MapGet("/", () => games);

// GET /games/{id}
group.MapGet("/{id}", (int id) =>
{
    Game? game = games.Find(game => game.Id == id);

    if (game == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(game);
})
.WithName("GetGame");

// POST /games
group.MapPost("/", (Game game) =>
{
    game.Id = games.Max(game => game.Id) + 1;
    games.Add(game);

    return Results.CreatedAtRoute("GetGame", new { id = game.Id }, game);
});

// PUT /games/{id}
group.MapPut("/{id}", (int id, Game updatedGame) =>
{
    Game? game = games.Find(game => game.Id == id);

    if (game == null)
    {
        updatedGame.Id = id;
        games.Add(updatedGame);
        return Results.CreatedAtRoute("GetGame", new { id = updatedGame.Id }, updatedGame);
    }

    game.Name = updatedGame.Name;
    game.Genre = updatedGame.Genre;
    game.Price = updatedGame.Price;
    game.ReleaseDate = updatedGame.ReleaseDate;

    return Results.NoContent();
});

// DELETE /games/{id}
group.MapDelete("/{id}", (int id) =>
{
    Game? game = games.Find(game => game.Id == id);

    if (game == null)
    {
        return Results.NotFound();
        // return Results.NoContent();
    }

    games.Remove(game);
    return Results.NoContent();
});

app.Run();
