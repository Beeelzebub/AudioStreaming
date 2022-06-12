

namespace AudioStreaming.Application.Abstractions.Services.BlobStorage
{
    public interface ITrackBlobService
    {
        Task<string> UploadAsync(int releaseId, string fileName, Stream fileData);

        Task<Stream?> GetStreamAsync(string path);

        Task DeleteIfExistsAsync(string path);
    }
}
