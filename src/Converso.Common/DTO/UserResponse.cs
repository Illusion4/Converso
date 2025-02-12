namespace SnapTalk.Common.DTO;

public record UserResponse(Guid Id, string Email, string UserName, string FirstName,
    string LastName, string Bio, string ProfilePictureUrl);