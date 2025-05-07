using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JocMemory.Services
{
    public class CategoryService
    {
        public List<string> LoadStandardImagePaths(string categoryFolder)
        {
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, categoryFolder);

            if (!Directory.Exists(fullPath))
                return new List<string>();

            var allImages = Directory.GetFiles(fullPath)
                .Where(f => f.EndsWith(".jpg") || f.EndsWith(".png") || f.EndsWith(".gif"))
                .ToList();

            var rnd = new Random();
            return allImages.OrderBy(_ => rnd.Next()).ToList();
        }
    }

}
