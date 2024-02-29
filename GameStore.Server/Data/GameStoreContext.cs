using GameStore.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Server.Data;

public class GameStoreContext : DbContext
{
    public GameStoreContext(DbContextOptions<GameStoreContext> options) : base(options)
    {
    }

    public DbSet<Game> Games => Set<Game>();
}