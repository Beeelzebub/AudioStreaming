

namespace MusicStreaming.Application.Abstractions.Services.BlobStorage
{
    public interface ISongBlobService
    {
        Task<string> UploadAsync(string releaseName, string fileName, Stream fileData);
        Task<string> GetLinkAsync(string path, string filename);
        Task<Stream> DownloadAsync(string path);
        Task DeleteIfExistsAsync(string path);
    }
}
