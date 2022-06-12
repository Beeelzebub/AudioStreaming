using AudioStreaming.Application.Abstractions.Services.BlobStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AudioStreaming.Infrastructure.Services.BlobStorage
{
    public class CoverBlobService : BlobServiceBase, ICoverBlobService
    {
        private const string ContainerName = "covers";

        public CoverBlobService(IConfiguration configuration, ILogger<BlobServiceBase> logger)
            : base(configuration, logger, ContainerName)
        {

        }

        public Task DeleteIfExistsAsync(string path)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetLinkAsync(string path, string filename)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UploadPlaylistCoverAsync(int playlistId, Stream fileData)
        {
            var path = $"playlists/{playlistId}.png";

            var uri = await UploadBlobAsync(path, fileData, "image/png");

            return uri != null ? uri.AbsoluteUri : "";
        }

        public async Task<string> UploadReleaseCoverAsync(int releaseId, Stream fileData)
        {
            var path = $"releases/{releaseId}.png";

            var uri = await UploadBlobAsync(path, fileData, "image/png");

            return uri != null ? uri.AbsoluteUri : "";
        }
    }
}
