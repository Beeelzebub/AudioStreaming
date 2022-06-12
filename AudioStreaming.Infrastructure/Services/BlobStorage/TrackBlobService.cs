using AudioStreaming.Application.Abstractions.Services.BlobStorage;
using AudioStreaming.Common.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AudioStreaming.Infrastructure.Services.BlobStorage
{
    public class TrackBlobService : BlobServiceBase, ITrackBlobService
    {
        private const string ContainerName = "tracks";

        public TrackBlobService(IConfiguration configuration, ILogger<BlobServiceBase> logger)
            : base(configuration, logger, ContainerName)
        {

        }

        public Task DeleteIfExistsAsync(string path)
        {
            return DeleteBlobIfExistsAsync(path);
        }

        public async Task<string> UploadAsync(int releaseId, string fileName, Stream fileData)
        {
            var path = $"{releaseId}/{fileName}";

            var uri = await UploadBlobAsync(path, fileData, "audio/mpeg");
            

            return uri != null ? uri.LocalPath.GetTrackPathInStorage() : "";
        }

        public async Task<Stream?> GetStreamAsync(string path)
        {
            var blobClient = _blobContainer.GetBlobClient(path);

            return await blobClient.ExistsAsync() ? await blobClient.OpenReadAsync() : null;
        }
    }
}
