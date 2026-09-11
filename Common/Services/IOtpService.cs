namespace exam_system.Common.Services;

public interface IOtpService
{
    string GenerateOtp();
    string HashOtp(string otp);
    bool VerifyOtp(string plainOtp, string hashedOtp);
}
