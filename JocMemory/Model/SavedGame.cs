using System.Collections.Generic;

namespace JocMemory.Model
{
    public class SavedGame
    {
        public string CategoryName { get; set; }
        public int Rows { get; set; }
        public int Cols { get; set; }
        public int RemainingSeconds { get; set; }
        public int ElapsedSeconds { get; set; }
        public List<SavedCard> Cards { get; set; }
    }

    public class SavedCard
    {
        public string ImagePath { get; set; }
        public bool IsFlipped { get; set; }
        public bool IsMatched { get; set; }
    }
}
