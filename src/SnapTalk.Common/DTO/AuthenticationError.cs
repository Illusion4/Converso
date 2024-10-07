using System.Xml;

namespace SnapTalk.Common.DTO;

public static class AuthenticationError
{
    public static readonly Error UsernameAlreadyExists = new("USERNAME_ALREADY_EXISTS");
    
    public static readonly Error InvalidOtpCode = new("INVALID_OTP_CODE");
}