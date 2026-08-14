namespace EmployeeManagement.Domain.Models;

public class JwtSettings
{
    public const string SectionName = nameof(JwtSettings);

    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string SigningKey { get; set; } = string.Empty;
    public int ExpirationMinutes { get; set; } = 60;

    public void CheckSettings()
    {
        if (string.IsNullOrWhiteSpace(Issuer))
            throw new InvalidOperationException(
                $"A value is required in '{SectionName}:{nameof(Issuer)}'.");

        if (string.IsNullOrWhiteSpace(Audience))
            throw new InvalidOperationException(
                $"A value is required in '{SectionName}:{nameof(Audience)}'.");

        if (SigningKey.Length < 32)
            throw new InvalidOperationException(
                $"'{SectionName}:{nameof(SigningKey)}' must contain at least 32 characters.");

        if (ExpirationMinutes <= 0)
            throw new InvalidOperationException(
                $"'{SectionName}:{nameof(ExpirationMinutes)}' must be greater than zero.");
    }
}
