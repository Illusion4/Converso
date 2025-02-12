namespace SnapTalk.Common.DTO;

public sealed record Error(string Code, string? Description = null, Dictionary<string, object>? Meta = null);