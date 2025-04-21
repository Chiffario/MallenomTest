namespace MallenomTest.Infrastructure;

/// <summary>
/// Represents a database connection string
/// </summary>
public class DatabaseConnection
{
    #region .ctor

    internal DatabaseConnection()
    {
    }

    /// <summary>
    /// Create a database connection string
    /// </summary>
    /// <param name="server">Server address</param>
    /// <param name="port">Server port</param>
    /// <param name="database">Database name</param>
    /// <param name="username">Database login user</param>
    /// <param name="password">Database login password</param>
    public DatabaseConnection(string server, string port, string database, string username, string password)
    {
        Server = new EnvironmentVariableConfigValue(server);
        Port = new EnvironmentVariableConfigValue(port);
        Database = new EnvironmentVariableConfigValue(database);
        Username = new EnvironmentVariableConfigValue(username);
        Password = new EnvironmentVariableConfigValue(password);
    }
    
    #endregion

    #region Properties

    /// <summary>
    /// Get server host
    /// </summary>
    public EnvironmentVariableConfigValue Server { get; init; } = new EnvironmentVariableConfigValue()
    {
        EnvironmentVariableName = "DB_HOST",
        FallbackValue = "http://database",
    };

    /// <summary>
    /// Get database login user
    /// </summary>
    public EnvironmentVariableConfigValue Username { get; init; } = new EnvironmentVariableConfigValue()
    {
        EnvironmentVariableName = "DB_USERNAME",
        FallbackValue = "user"
    };
    
    /// <summary>
    /// Get database name
    /// </summary>
    public EnvironmentVariableConfigValue Database { get; init; } = new EnvironmentVariableConfigValue()
    {
        EnvironmentVariableName = "DB_DATABASE",
        FallbackValue = "mallenom"
    };
    
    /// <summary>
    /// Get database server port
    /// </summary>
    public EnvironmentVariableConfigValue Port { get; init; } = new EnvironmentVariableConfigValue()
    {
        EnvironmentVariableName = "DB_PORT",
        FallbackValue = "5432"
    };
    
    public EnvironmentVariableConfigValue Password { get; init; } = new EnvironmentVariableConfigValue()
    {
        EnvironmentVariableName = "DB_PASSWORD",
        FallbackValue = "mallenom"
    };

    #endregion
    
    #region Methods

    /// <summary>Create an instance of <see cref = "DatabaseConnection" />.</summary>
    /// <returns>Instance of <see cref = "DatabaseConnection" />.</returns>
    public static DatabaseConnection Create() => new();

    #endregion

}