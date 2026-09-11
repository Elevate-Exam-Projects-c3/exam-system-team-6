namespace exam_system.Common.Services;

public sealed class PasswordHasher : IPasswordHasher
{
    private readonly int _saltRounds;

    public PasswordHasher(IConfiguration configuration)
    {
        var configValue = configuration["Bcrypt:SaltRounds"];
        _saltRounds = int.TryParse(configValue, out var rounds) && rounds is >= 4 and <= 31
            ? rounds
            : 12;
    }

    public string Hash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, workFactor: _saltRounds);
    }

    public bool Verify(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}
