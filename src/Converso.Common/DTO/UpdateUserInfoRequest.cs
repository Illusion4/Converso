namespace SnapTalk.Common.DTO;

public record UpdateUserInfoRequest(
    string Username,
    string FirstName,
    string? LastName,
    string? Bio);