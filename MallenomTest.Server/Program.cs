using MallenomTest.Database;
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
        var dbConfig = builder.Configuration.GetSection("Database");
        var connectionString = new NpgsqlConnectionStringBuilder
        {
            Host = dbConfig["Host"],
            Port = int.Parse(dbConfig["Port"]!),
            Database = dbConfig["Database"],
            Username = dbConfig["Username"],
            Password = dbConfig["Password"]
        };
        
        builder.Services.AddDbContext<DatabaseContext>(op =>
            op.UseNpgsql(connectionString.ConnectionString));
        builder.Services.AddScoped<IImagesService, ImagesService>();
        
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
        
        
        app.Run();
    }
}