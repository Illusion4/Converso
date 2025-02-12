namespace SnapTalk.BLL.Interfaces;

public interface IOtpService
{
    public string GenerateOtp();
    
    public string HashOtp(string otp);
    
    public bool VerifyOtp(string otp, string hashedOtp);
}