namespace SnapTalk.BLL.Interfaces;

public interface IAvatarService
{
    byte[] GenerateAvatar(string avatarText, string color);
}