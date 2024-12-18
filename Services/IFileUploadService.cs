namespace LibraryMgmt.Services
{
    public interface IFileUploadService
    {
        Task<string> UploadFileAsync(IFormFile file, string destinationFolder);
    }
}
