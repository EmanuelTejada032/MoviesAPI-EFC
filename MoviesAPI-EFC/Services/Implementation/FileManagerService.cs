using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MoviesAPI_EFC.Services.Contract;

namespace MoviesAPI_EFC.Services.Implementation
{
    public class FileManagerService : IFileManager
    {
        private readonly string _azureStorageConnString;

        public FileManagerService(IConfiguration configuration)
        {
            _azureStorageConnString = configuration.GetConnectionString("AzureStorage");
        }

        public async Task<string> EditFile(byte[] content, string extension, string container, string contentType, string path)
        {
            await DeleteFile(path, container);
            return await UploadFile(content, extension, container, contentType);
        }

        public async Task<string> UploadFile(byte[] content, string extension, string container, string contentType)
        {
            var client = new BlobContainerClient(_azureStorageConnString, container);
            await client.CreateIfNotExistsAsync();
            await client.SetAccessPolicyAsync(PublicAccessType.Blob);

            var fileName = $"{Guid.NewGuid()}{extension}";
            var blob = client.GetBlobClient(fileName);

            var blobUploadOptions = new BlobUploadOptions();
            var blobHttpHeaders = new BlobHttpHeaders();
            blobHttpHeaders.ContentType = contentType;
            blobUploadOptions.HttpHeaders = blobHttpHeaders;

            await blob.UploadAsync(new BinaryData(content), blobUploadOptions);

            return blob.Uri.ToString();
        }

        public async Task DeleteFile(string path, string container)
        {
            if (string.IsNullOrEmpty(path)) return;

            var client = new BlobContainerClient(_azureStorageConnString, container);
            await client.CreateIfNotExistsAsync();
            var file = Path.GetFileName(path);
            var blob = client.GetBlobClient(file);

            await blob.DeleteIfExistsAsync();

        }
    }
}
