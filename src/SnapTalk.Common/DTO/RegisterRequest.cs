namespace SnapTalk.Common.DTO;

public record RegisterRequest(string Email, string UserName, string FirstName, string LastName, string Password, string Code);