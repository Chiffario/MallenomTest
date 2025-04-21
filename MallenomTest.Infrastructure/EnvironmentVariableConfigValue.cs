namespace MallenomTest.Infrastructure;

using System.Text.RegularExpressions;

/// <summary>Config value taken from the environment</summary>
public partial class EnvironmentVariableConfigValue
{
    #region .ctor

    /// <summary>Create <see cref = "EnvironmentVariableConfigValue" />.</summary>
    public EnvironmentVariableConfigValue()
    {
        EnvironmentVariableName = string.Empty;
    }

    /// <summary>Create <see cref = "EnvironmentVariableConfigValue" />.</summary>
    /// <param name = "declaring">Configuration value</param>
    public EnvironmentVariableConfigValue(string declaring) : this()
    {
        var match = EnvironmentVariableConfigValue.Regex().Match(declaring);

        if (!match.Success)
        {
            FallbackValue = declaring;
        }
        else
        {
            var envNameGroup = match.Groups["envName"];
            var envNameAltGroup = match.Groups["envNameWithoutFallback"];
            var fallbackGroup = match.Groups["fallback"];

            if (envNameGroup.Success)
            {
                EnvironmentVariableName = envNameGroup.Value;
            }
            else if (envNameAltGroup.Success)
            {
                EnvironmentVariableName = envNameAltGroup.Value;
            }

            if (fallbackGroup.Success)
            {
                FallbackValue = fallbackGroup.Value;
            }
        }
    }

    #endregion

    #region Properties

    /// <summary>
    /// Return the fallback value of the environment variable
    /// </summary>
    public string? FallbackValue { get; init; }

    /// <summary>Set env. variable name</summary>
    public string EnvironmentVariableName { get; init; }

    #endregion

    #region Methods

    /// <summary>Get env. variable name</summary>
    public string? GetValue()
    {
        if (Environment.GetEnvironmentVariable(EnvironmentVariableName) is { } value)
        {
            return value;
        }

        return FallbackValue;
    }

    public static implicit operator EnvironmentVariableConfigValue(string value) => new(value);

    [GeneratedRegex(@"(\$env:(?<envName>(.+))(\((?<fallback>(.*?))\)))|(\$env:(?<envNameWithoutFallback>(.+)))", RegexOptions.Compiled)]
    private static partial Regex Regex();

    #endregion
}
