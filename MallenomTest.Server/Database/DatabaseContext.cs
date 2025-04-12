using MallenomTest.Database.Models;
using Microsoft.EntityFrameworkCore;
namespace MallenomTest.Database;

public sealed class DatabaseContext : DbContext
{
    public DbSet<ImageModel> Images { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    private DatabaseContext()
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ImageModel>().HasIndex(img => img.Id);
        base.OnModelCreating(modelBuilder);
    }
}