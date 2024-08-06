namespace SnapTalk.BLL.Interfaces;

public interface ICurrentContextProvider
{
    public Guid CurrentUserId { get; }
}