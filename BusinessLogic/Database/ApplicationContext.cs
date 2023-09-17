using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Database;

/// <summary>
/// EntityFramework application context.
/// </summary>
public class ApplicationContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationContext"/> class.
    /// </summary>
    /// <param name="options">Options for datacontext.</param>
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets lobby DbSet for access to lobbies in database.
    /// </summary>
    /// <value>Lobby DbSet.</value>
    required public DbSet<LobbyAccess> Lobby { get; set; }

    /// <inheritdoc />
    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LobbyAccess>()
            .Property(f => f.Id)
            .ValueGeneratedOnAdd();
    }
}
