namespace SnapTalk.BLL.Interfaces;

public interface IBlobService
{
    string GetBlobUrl(string fileName);

    Task UploadFileBlobAsync(Stream stream, string contentType, string uniqueName);

    Task<bool> DeleteBlobAsync(string fileName);
}