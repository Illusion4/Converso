namespace SnapTalk.BLL.Services;

public class BlobStorageSettings(string blobConnectionString, string blobContainerName, string blobAccessUrl)
{
    public const string Storage = "Storage";

    public readonly string BlobConnectionString = blobConnectionString;

    public readonly string BlobContainerName = blobContainerName;

    public readonly string BlobAccessUrl = blobAccessUrl;
}