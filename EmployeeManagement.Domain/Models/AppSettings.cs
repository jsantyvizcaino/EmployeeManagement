namespace EmployeeManagement.Domain.Models;

public class AppSettings
{
    public const string SectionName = nameof(AppSettings);

    public string[] BaseDomain { get; set; } = [];
    public int PasswordMinLength { get; set; } = 8;
    public int PasswordMaxLength { get; set; } = 50;
    public string FrontendUrl { get; set; } = string.Empty;

    public void CheckSettings()
    {
        if (BaseDomain.Length == 0 || BaseDomain.Any(string.IsNullOrWhiteSpace))
            throw new InvalidOperationException(
                $"At least one valid value is required in '{SectionName}:{nameof(BaseDomain)}'.");

        if (string.IsNullOrWhiteSpace(FrontendUrl))
            throw new InvalidOperationException(
                $"A value is required in '{SectionName}:{nameof(FrontendUrl)}'.");

        if (PasswordMinLength <= 0)
            throw new InvalidOperationException(
                $"'{SectionName}:{nameof(PasswordMinLength)}' must be greater than zero.");

        if (PasswordMaxLength < PasswordMinLength)
            throw new InvalidOperationException(
                $"'{SectionName}:{nameof(PasswordMaxLength)}' must be greater than or equal to '{nameof(PasswordMinLength)}'.");
    }
}
