using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace WUCSA.Web.Utils
{
    public class PDFFileHelper
    {
        private readonly IWebHostEnvironment _env;
        public PDFFileHelper(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> SaveFile(IFormFile formFile, string fileName, string subFolder)
        {
            var fileFullName = $"{fileName}{".pdf"}";
            string filePath = Path.Combine(_env.WebRootPath, "pdfFiles", subFolder, fileFullName);
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
            }
            return $"/pdfFiles/{subFolder}/{fileFullName}";
        }

        public void DeleteFile(string filePath, string subFolder)
        {
            var fileName = Path.GetFileName(filePath);

            var absolutePath = Path.Combine(_env.WebRootPath, "pdfFiles", subFolder, fileName);
            if (File.Exists(absolutePath))
            {
                File.Delete(absolutePath);
            }
        }
    }
}
