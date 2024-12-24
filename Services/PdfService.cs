using Microsoft.AspNetCore.Mvc;

namespace LibraryMgmt.Services
{
    public class PdfService:IPdfService
    {
        public string GetPdfFile(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "book-files ", fileName);
            return filePath;
        }
    }
}
