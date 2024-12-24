using Microsoft.AspNetCore.Mvc;

namespace LibraryMgmt.Services
{
    public interface IPdfService
    {
        string GetPdfFile(string fileName);
    }
}
