namespace exam_system.Common.Services;

public class PasswordHasher : IPasswordHasher
{
    private const int SaltRounds = 12;

    public string Hash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, workFactor: SaltRounds);
    }

    public bool Verify(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}
