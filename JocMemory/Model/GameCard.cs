using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace JocMemory.Model
{
    public class GameCard : INotifyPropertyChanged
    {
        private string imagePath;
        private bool isFlipped;
        private bool isMatched;

        public string ImagePath
        {
            get => imagePath;
            set { imagePath = value; OnPropertyChanged(); }
        }

        public bool IsFlipped
        {
            get => isFlipped;
            set { isFlipped = value; OnPropertyChanged(); }
        }

        public bool IsMatched
        {
            get => isMatched;
            set { isMatched = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
