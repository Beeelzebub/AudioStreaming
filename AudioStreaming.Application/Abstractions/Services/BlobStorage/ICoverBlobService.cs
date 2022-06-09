namespace AudioStreaming.Application.Abstractions.Services.BlobStorage
{
    public interface ICoverBlobService
    {
        Task<string> UploadReleaseCoverAsync(int releaseId, Stream fileData);

        Task<string> UploadPlaylistCoverAsync(int playlistId, Stream fileData);

        Task<string> GetLinkAsync(string path, string filename);

        Task DeleteIfExistsAsync(string path);
    }
}
