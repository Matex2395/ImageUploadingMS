namespace ImageMS.Interfaces
{
    public interface IGoogleCloudStorageService
    {
        Task<string> UploadImageAsync(IFormFile file, string folderName);
    }
}
