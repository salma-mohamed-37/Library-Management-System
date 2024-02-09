using Humanizer;
using System.Security.Policy;

namespace backend.Handlers
{
    public class ImageHandler
    {
        public async Task SaveImageFile(IFormFile image, string uniqueName, string modelName)
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "MyStaticFiles", "Images",modelName ,uniqueName);
            using var fs = new FileStream(filePath, FileMode.Create);
            await image.CopyToAsync(fs);
        }

        public void DeleteImage(string imageName, string modelName)
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "MyStaticFiles", "Images", modelName, imageName);
            File.Delete(filePath);
        }
    }
}
