using System.Security.Cryptography;
using System.Text;

namespace exam_system.Common.Services;

public sealed class OtpService : IOtpService
{
    public string GenerateOtp()
    {
        return RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
    }

    public string HashOtp(string otp)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(otp));
        return Convert.ToHexString(bytes);
    }

    public bool VerifyOtp(string plainOtp, string hashedOtp)
    {
        var computedHash = HashOtp(plainOtp);
        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(computedHash),
            Encoding.UTF8.GetBytes(hashedOtp)
        );
    }
}
