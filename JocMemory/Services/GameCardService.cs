using JocMemory.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace JocMemory.Services
{
    public class GameCardService
    {
        public List<GameCard> GenerateCards(string folderPath, int totalCards)
        {
            var categoryService = new CategoryService();
            var images = categoryService.LoadStandardImagePaths(folderPath)
                                        .Distinct()
                                        .ToList();

            int numPairs = totalCards / 2;

            if (images.Count < numPairs)
            {
                numPairs = images.Count;
                totalCards = numPairs * 2;
            }

            var selectedImages = images.OrderBy(_ => Guid.NewGuid())
                                       .Take(numPairs)
                                       .ToList();

            var duplicated = selectedImages.SelectMany(img => new[]
            {
        new GameCard { ImagePath = img },
        new GameCard { ImagePath = img }
    }).ToList();

            return duplicated.OrderBy(_ => Guid.NewGuid()).ToList();
        }

    }
}
