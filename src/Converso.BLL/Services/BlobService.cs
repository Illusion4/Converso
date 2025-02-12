using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using SnapTalk.BLL.Interfaces;

namespace SnapTalk.BLL.Services;

public class BlobService(BlobContainerClient blobClient) : IBlobService
{
    public string GetBlobUrl(string fileName)
    {
        var client = blobClient.GetBlobClient(fileName);

        return client.Uri.AbsoluteUri;
    }

    public async Task UploadFileBlobAsync(Stream stream, string contentType, string uniqueName)
    {
        var client = blobClient.GetBlobClient(uniqueName);
        var headers = new BlobHttpHeaders { ContentType = contentType };
        await client.UploadAsync(stream, headers);
    }

    public async Task<bool> DeleteBlobAsync(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentNullException(nameof(fileName));
        }

        var client = blobClient.GetBlobClient(fileName);

        var result = await client.DeleteIfExistsAsync();

        return result.Value;
    }
}