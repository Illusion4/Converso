namespace SnapTalk.Domain.Entities;

public class SignupCodeEntity
{
    public required string Email { get; set; }
    
    public required int Code { get; set; }
}