using AudioStreaming.Application.Abstractions.Services.BlobStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AudioStreaming.Infrastructure.Services.BlobStorage
{
    public class TrackBlobService : BlobServiceBase, ITrackBlobService
    {
        private const string ContainerName = "Tracks";

        public TrackBlobService(IConfiguration configuration, ILogger logger)
            : base(configuration, logger, ContainerName)
        {

        }

        public Task DeleteIfExistsAsync(string path)
        {
            return DeleteBlobIfExistsAsync(path);
        }

        public async Task<string> UploadAsync(string releaseId, string fileName, Stream fileData)
        {
            var path = $"{releaseId}/{fileName}";

            return await UploadBlobAsync(path, fileData, "audio/mpeg");
        }

        public async Task<Stream?> GetStreamAsync(string path)
        {
            var blobClient = _blobContainer.GetBlobClient(path);
            
            return await blobClient.ExistsAsync() ? await blobClient.OpenReadAsync() : null;
        }
    }
}
