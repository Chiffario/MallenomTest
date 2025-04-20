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
        modelBuilder.Entity<ImageModel>(entity =>
        {
            entity.ToTable("Images");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .IsRequired();

            entity.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(e => e.FileType)
                .HasColumnName("fileType")
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(e => e.Data)
                .HasColumnName("image")
                .IsRequired();
        });
        base.OnModelCreating(modelBuilder);
    }
}