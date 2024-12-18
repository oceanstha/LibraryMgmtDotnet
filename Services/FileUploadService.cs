namespace LibraryMgmt.Services
{
    public class FileUploadService : IFileUploadService
    {
        public async Task<string> UploadFileAsync(IFormFile file, string destinationFolder)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file provided.");

            if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
                throw new ArgumentException("Only PDF files are allowed.");


            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", destinationFolder);
            Directory.CreateDirectory(uploadsPath);

            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadsPath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/{destinationFolder}/{uniqueFileName}";
        }
    }
}
