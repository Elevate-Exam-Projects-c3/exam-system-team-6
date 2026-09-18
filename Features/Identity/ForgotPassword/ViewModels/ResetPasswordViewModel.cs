namespace exam_system.Features.Identity.ForgotPassword.ViewModels;

public class ResetPasswordViewModel
{
    public string ResetToken { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
}
