namespace AudioStreaming.Application.Abstractions.Services.BlobStorage
{
    public interface IBlobService
    {
        Task<bool> Upload(string fileName, Stream fileData, string fileMimeType);
        Task<string> GetLink(string path, string filename);
        Task<Stream> Download(string path);
        Task DeleteIfExists(string path);
    }
}
