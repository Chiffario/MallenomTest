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
                .HasColumnType("integer")
                .IsRequired();

            entity.Property(e => e.Name)
                .HasColumnName("name")
                .HasColumnType("character varying(20)")
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(e => e.FileType)
                .HasColumnName("fileType")
                .HasColumnType("character varying(200)")
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(e => e.Data)
                .HasColumnName("image")
                .HasColumnName("bytea")
                .IsRequired();
        });
        base.OnModelCreating(modelBuilder);
    }
}