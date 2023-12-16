using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TazaFood.Core.Services;

namespace TazaFood.Service
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task DeleteIamge(string imagePath)
        {
            if (!string.IsNullOrEmpty(imagePath))
            {
                string fulllPath = Path.Combine(_environment.WebRootPath, imagePath.TrimStart('/'));
                if (File.Exists(fulllPath))
                {
                    File.Delete(fulllPath);
                }
            }
        }
    }
}
