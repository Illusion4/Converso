using System.Security.Cryptography;
using System.Text;
using SnapTalk.BLL.Interfaces;

namespace SnapTalk.BLL.Services;

public class OtpService : IOtpService
{
    public string GenerateOtp() =>
        RandomNumberGenerator.GetInt32(100000, 999999).ToString();
    
    public string HashOtp(string otp)
    {
        return ComputeSha256Hash(otp);
    }

    public bool VerifyOtp(string otp, string hashedOtp) =>
        HashOtp(otp) == hashedOtp;

    private string ComputeSha256Hash(string input)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(input);
        return ComputeSha256Hash(bytes);
    }
    
    private string ComputeSha256Hash(byte[] bytes)
    {
        using var sha256 = SHA256.Create();
        byte[] hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}