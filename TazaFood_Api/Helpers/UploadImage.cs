using Microsoft.AspNetCore.Http;

namespace TazaFood_Api.Helpers
{
    public  static class UploadImage
    {
        public static async Task<string> SaveImage(IFormFile formFile ,string source, string imageName)
        {

            string filePath = Path.Combine(source, "images", "products");

            // check if the folder exists or not
            if (!Directory.Exists(filePath))
            {
                // create folder with this path 
                Directory.CreateDirectory(filePath);
            }

            // Generate A Unique FileName 
            string fileName = $"{imageName.Replace(' ', '_')}_{DateTime.Now.Ticks}{Path.GetExtension(formFile.FileName)}";

            // Combine the base directory and the unique file name to get the full file path
            string imagePath = Path.Combine(filePath, fileName);

            //Check If the File with the same name IS Already Exists
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }

            // Create a FileStream and asynchronously copy the contents of the uploaded image to it
            using (FileStream stream = File.Create(imagePath))
            {
                await formFile.CopyToAsync(stream);
            }

            // Replace backslashes with forward slashes in the image path
            imagePath = imagePath.Replace("\\", "/");

            return imagePath;
        }
    }
}
