using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace MCV_Mini_Project.Helpers
{
    public static class FileHelper
    {
        /// <summary>
        /// Saves an uploaded file to wwwroot/images and returns the file name only
        /// (same format the old views use: ~/images/@fileName).
        /// </summary>
        public static async Task<string?> SaveImageAsync(IFormFile? file, IWebHostEnvironment env)
        {
            if (file is null || file.Length == 0) return null;

            var uploadsFolder = Path.Combine(env.WebRootPath, "images");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return uniqueName;
        }

        /// <summary>
        /// Extracts the file name from any stored value:
        /// teacher.jpg | images/teacher.jpg | /images/teacher.jpg | ~/images/teacher.jpg
        /// </summary>
        public static string? GetFileName(string? stored)
        {
            if (string.IsNullOrWhiteSpace(stored)) return null;

            var value = stored.Replace('\\', '/').Trim();
            if (value.StartsWith("~/")) value = value[2..];
            value = value.TrimStart('/');

            if (value.StartsWith("images/", StringComparison.OrdinalIgnoreCase))
                value = value[7..];

            return string.IsNullOrWhiteSpace(value) ? null : Path.GetFileName(value);
        }

        /// <summary>
        /// Returns a web path that works from any page, including Admin.
        /// </summary>
        public static string GetImageUrl(string? stored)
        {
            var name = GetFileName(stored);
            return string.IsNullOrEmpty(name) ? "/images/no-image.png" : $"/images/{name}";
        }

        /// <summary>
        /// Deletes an uploaded file from wwwroot/images. Seed images (no GUID prefix) are kept.
        /// </summary>
        public static void DeleteImage(string? stored, IWebHostEnvironment env)
        {
            var name = GetFileName(stored);
            if (string.IsNullOrEmpty(name)) return;

            var prefix = name.Split('_')[0];
            if (!Guid.TryParse(prefix, out _)) return;

            var fullPath = Path.Combine(env.WebRootPath, "images", name);
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
    }
}
