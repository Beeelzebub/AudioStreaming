using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AudioStreaming.Infrastructure.Services.BlobStorage
{
    public abstract class BlobServiceBase
    {
        protected readonly ILogger _logger;
        protected readonly BlobContainerClient _blobContainer;

        public BlobServiceBase(IConfiguration configuration, ILogger logger, string containerName)
        {
            _logger = logger;
            var storageAccount = new BlobServiceClient(configuration["StorageConnectionString"]);
            _blobContainer = storageAccount.GetBlobContainerClient(containerName);
        }

        protected async Task<string> UploadBlobAsync(string path, Stream fileData, string fileMimeType)
        {
            try
            {
                await _blobContainer.CreateIfNotExistsAsync();
                await _blobContainer.SetAccessPolicyAsync(PublicAccessType.Blob);

                if (path != null && fileData != null)
                {
                    var blobClient = _blobContainer.GetBlobClient(path);
                    await blobClient.UploadAsync(fileData, new BlobHttpHeaders() { ContentType = fileMimeType });

                    return blobClient.Uri.AbsolutePath;
                }
                else
                {
                    throw new ArgumentException("Path string cannot be null or empty.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to upload blob to storage:" + ex.Message);

                return string.Empty;
            }
        }

        protected async Task<Stream?> GetBlobStreamAsync(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    throw new ArgumentException("Path string cannot be null or empty.");
                }

                var blobClient = _blobContainer.GetBlobClient(path);
                return await blobClient.OpenReadAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get stream:" + ex.Message);

                return null;
            }
        }

        protected Task DeleteBlobIfExistsAsync(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    throw new ArgumentException("Path string cannot be null or empty.");
                }
                if (!Uri.TryCreate(path, UriKind.RelativeOrAbsolute, out var uri))
                {
                    throw new ArgumentException("Invalid path.");
                }

                return _blobContainer.DeleteBlobIfExistsAsync(uri.Segments.LastOrDefault());
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to delete blob:" + ex.Message);

                return Task.CompletedTask;
            }
        }
    }
}
