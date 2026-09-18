namespace exam_system.Features.Identity.ForgotPassword;

public class VerifyResetOtpResponse
{
    public string ResetToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public string Message { get; set; } = string.Empty;
}
