using MallenomTest.Infrastructure;
using Npgsql;

namespace MallenomTest.Configuration;

/// <summary>
/// Extension for <see cref="ConfigurationManager"/>
/// </summary>
internal static class ConfigurationManagerExtension
{
    public static NpgsqlConnection GetPostgresConnectionFromEnv(this ConfigurationManager configurationManager)
    {
        ArgumentNullException.ThrowIfNull(configurationManager);
        var databaseConnection = configurationManager.GetSection("Database")
            .Get<DatabaseConnection>();
        ArgumentNullException.ThrowIfNull(databaseConnection);

        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = databaseConnection.Server.GetValue(),
            Username = databaseConnection.Username.GetValue(),
            Database = databaseConnection.Database.GetValue(),
            Port = int.Parse(databaseConnection.Port.GetValue()!),
            Password = databaseConnection.Password.GetValue()
        };

        return new NpgsqlConnection(builder.ConnectionString);

    }
}