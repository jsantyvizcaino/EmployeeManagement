namespace EmployeeManagement.Domain.Models;

public class DbSettings
{
    public const string SectionName = nameof(DbSettings);

    public string ConnectionString { get; set; } = string.Empty;
    public int CommandTimeoutSeconds { get; set; } = 30;

    public void CheckSettings()
    {
        if (string.IsNullOrWhiteSpace(ConnectionString))
            throw new InvalidOperationException(
                $"A value is required in '{SectionName}:{nameof(ConnectionString)}'.");

        if (CommandTimeoutSeconds <= 0)
            throw new InvalidOperationException(
                $"'{SectionName}:{nameof(CommandTimeoutSeconds)}' must be greater than zero.");
    }
}
