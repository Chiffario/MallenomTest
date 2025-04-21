using MallenomTest.Configuration;
using MallenomTest.Database;
using MallenomTest.Infrastructure;
using MallenomTest.Services;
using MallenomTest.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace MallenomTest;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddLogging();



        // Add server state related services
        builder.Services.AddDbContext<DatabaseContext>(op =>
            op.UseNpgsql(builder.Configuration.GetPostgresConnectionFromEnv()),
            ServiceLifetime.Transient);
        builder.Services.AddTransient<IImagesService, ImagesService>();

        // Add endpoint-related services
        builder.Services.AddHealthChecks();
        builder.Services.AddProblemDetails(); // Problem details allow for extra error information
        builder.Services.AddControllers();

        // Add API documentation services
        builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        app.UseHttpsRedirection();

        app.MapControllers();

        app.MapHealthChecks("/health");

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Apply any pending migrations to simplify deploys
        // Shouldn't be done if multi-instancing but this server isn't supposed to be multi-instanced
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<DatabaseContext>();
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }

        app.Run();
    }
}