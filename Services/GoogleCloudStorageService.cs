using Google.Cloud.Storage.V1;
using ImageMS.Interfaces;

namespace ImageMS.Services
{
    public class GoogleCloudStorageService : IGoogleCloudStorageService
    {
        private readonly StorageClient _storageClient;
        private readonly string _bucketName;

        public GoogleCloudStorageService(IConfiguration configuration)
        {
            _bucketName = configuration["GoogleCloud:BucketName"]!;
            var credentialsPath = configuration["GoogleCloud:CredentialsFile"];
            _storageClient = StorageClient.Create(Google
                                                    .Apis.Auth.OAuth2
                                                    .GoogleCredential
                                                    .FromFile(credentialsPath));
        }
        public async Task<string> UploadImageAsync(IFormFile file, string folderName)
        {
            // Validate if the Folder Name is Null or Empty
            if (string.IsNullOrWhiteSpace(folderName))
            {
                throw new ArgumentException("El nombre de la carpeta no puede estar vacío.", nameof(folderName));
            }

            // Build the name of the object with the folder
            var objectName = $"{folderName}/{Guid.NewGuid()}_{file.FileName}";
            using var stream = file.OpenReadStream();

            // Upload the File to the Bucket with the specified path
            await _storageClient.UploadObjectAsync(_bucketName, objectName, file.ContentType, stream);

            // Return the public URL of the file
            return $"https=//storage.googleapis.com/{_bucketName}/{objectName}";
        }
    }
}
