using JocMemory.Model;
using JocMemory.Services;
using JocMemory.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace JocMemory.ViewModel
{
    public class GameViewModel : INotifyPropertyChanged
    {
        public string BackgroundImagePath => "Images/b2.jpg";
        public string BackImagePath => "Images/c4.jpg";

        public ObservableCollection<ImageCategory> Categories { get; set; }
        public ObservableCollection<GameCard> GameCards { get; set; } = new ObservableCollection<GameCard>();

        private ImageCategory selectedCategory;
        public ImageCategory SelectedCategory
        {
            get => selectedCategory;
            set { selectedCategory = value; OnPropertyChanged(); }
        }

        private string selectedMode = "Standard";
        public string SelectedMode
        {
            get => selectedMode;
            set { selectedMode = value; OnPropertyChanged(); }
        }

        public string CurrentUserName { get; set; }

        private bool gameEnded = false;

        private int totalInitialSeconds;
        private System.Windows.Threading.DispatcherTimer gameTimer;
        private System.Diagnostics.Stopwatch stopwatch;

        public string RemainingTime
        {
            get
            {
                if (stopwatch == null)
                    return TimeSpan.FromSeconds(totalInitialSeconds).ToString(@"mm\:ss");

                int secondsLeft = totalInitialSeconds - (int)stopwatch.Elapsed.TotalSeconds;
                if (secondsLeft < 0) secondsLeft = 0;
                return TimeSpan.FromSeconds(secondsLeft).ToString(@"mm\:ss");
            }
        }

        private int gridRows = 4;
        private int gridCols = 4;
        public int GridRows
        {
            get => gridRows;
            set { gridRows = value; OnPropertyChanged(); }
        }

        public int GridCols
        {
            get => gridCols;
            set { gridCols = value; OnPropertyChanged(); }
        }

        private int customRows = 4;
        private int customCols = 4;
        public int CustomRows
        {
            get => customRows;
            set { customRows = value; OnPropertyChanged(); }
        }

        public int CustomCols
        {
            get => customCols;
            set { customCols = value; OnPropertyChanged(); }
        }

        private GameCard firstFlipped;
        public GameCard FirstFlipped
        {
            get => firstFlipped;
            set { firstFlipped = value; OnPropertyChanged(); }
        }

        private bool isChecking = false;

        public ICommand FlipCardCommand { get; }
        public ICommand StartNewGameCommand { get; }
        public ICommand SetModeCommand { get; }
        private RelayCommand saveGameCommand;
        public ICommand SaveGameCommand => saveGameCommand;
        private RelayCommand openGameCommand;
        public ICommand OpenGameCommand => openGameCommand;
        private RelayCommand showStatisticsCommand;
        public ICommand ShowStatisticsCommand => showStatisticsCommand;
        public ICommand ShowAboutCommand { get; }
        private RelayCommand exitCommand;
        public ICommand ExitCommand => exitCommand;

        public GameViewModel()
        {
            Categories = new ObservableCollection<ImageCategory>
            {
                new ImageCategory { Name = "Rabbits", FolderPath = "Images/Rabbits" },
                new ImageCategory { Name = "Eggs", FolderPath = "Images/Eggs" },
                new ImageCategory { Name = "Easter", FolderPath = "Images/Easter" }
            };

            FlipCardCommand = new RelayCommand(obj => FlipCard(obj as GameCard));
            StartNewGameCommand = new RelayCommand(_ => StartNewGame());
            SetModeCommand = new RelayCommand(param => SetGameMode(param));
            saveGameCommand = new RelayCommand(_ => SaveGame(), _ => GameCards.Any() && !gameEnded);
            openGameCommand = new RelayCommand(_ => LoadSavedGame(), _ => !string.IsNullOrWhiteSpace(CurrentUserName));
            showStatisticsCommand = new RelayCommand(_ => ShowStatistics());
            ShowAboutCommand = new RelayCommand(_ => ShowAbout());
            exitCommand = new RelayCommand(_ => ExitToLogin());
        }

        private void StartNewGame()
        {
            if (SelectedCategory == null)
            {
                MessageBox.Show("Select a category before starting the game.");
                return;
            }

            gameEnded = false;

            int rows = SelectedMode == "Custom" ? customRows : 4;
            int cols = SelectedMode == "Custom" ? customCols : 4;

            if (SelectedMode == "Custom")
            {
                var dialog = new CustomSizeDialog();
                if (dialog.ShowDialog() == true)
                {
                    rows = dialog.Rows;
                    cols = dialog.Cols;
                }
                else
                    return;
            }

            var timeDialog = new TimeInputDialog();
            if (timeDialog.ShowDialog() == true)
            {
                totalInitialSeconds = timeDialog.Minutes * 60;
                OnPropertyChanged(nameof(RemainingTime));

                stopwatch = System.Diagnostics.Stopwatch.StartNew();
                gameTimer = new System.Windows.Threading.DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(1)
                };
                gameTimer.Tick += GameTimer_Tick;
                gameTimer.Start();

                GridRows = rows;
                GridCols = cols;
                int totalCards = rows * cols;

                var service = new GameCardService();
                var cards = service.GenerateCards(SelectedCategory.FolderPath, totalCards);

                GameCards.Clear();
                foreach (var card in cards)
                    GameCards.Add(card);

                FirstFlipped = null;
                saveGameCommand.RaiseCanExecuteChanged();
            }
        }

        public void FlipCard(GameCard card)
        {
            if (gameEnded || card == null || card.IsMatched || card.IsFlipped || isChecking) return;

            card.IsFlipped = true;
            OnPropertyChanged(nameof(GameCards));

            if (FirstFlipped == null)
            {
                FirstFlipped = card;
            }
            else
            {
                isChecking = true;
                Task.Delay(1000).ContinueWith(_ =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (card.ImagePath == FirstFlipped.ImagePath)
                        {
                            card.IsMatched = FirstFlipped.IsMatched = true;
                        }
                        else
                        {
                            card.IsFlipped = FirstFlipped.IsFlipped = false;
                        }

                        FirstFlipped = null;
                        isChecking = false;
                        OnPropertyChanged(nameof(GameCards));

                        if (GameCards.All(c => c.IsMatched))
                        {
                            gameEnded = true;
                            gameTimer?.Stop();
                            UpdateStatistics(true);
                            new WinDialog().ShowDialog();
                        }
                    });
                });
            }
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(RemainingTime));

            if (stopwatch == null)
                return;

            int secondsLeft = totalInitialSeconds - (int)stopwatch.Elapsed.TotalSeconds;
            if (secondsLeft <= 0 && !gameEnded)
            {
                gameTimer.Stop();
                stopwatch.Stop();
                gameEnded = true;
                UpdateStatistics(false);
                EndGame();
            }
        }

        private void EndGame()
        {
            foreach (var card in GameCards)
                card.IsFlipped = true;

            new LoseDialog().ShowDialog();
            OnPropertyChanged(nameof(GameCards));
        }

        private void SaveGame()
        {
            if (string.IsNullOrWhiteSpace(CurrentUserName))
            {
                MessageBox.Show("The username is not set. The game cannot be saved.");
                return;
            }

            gameTimer?.Stop();

            var saveModel = new SavedGame
            {
                CategoryName = SelectedCategory?.Name,
                Rows = GridRows,
                Cols = GridCols,
                RemainingSeconds = totalInitialSeconds - (int)stopwatch.Elapsed.TotalSeconds,
                ElapsedSeconds = (int)stopwatch.Elapsed.TotalSeconds,
                Cards = GameCards.Select(c => new SavedCard
                {
                    ImagePath = c.ImagePath,
                    IsFlipped = c.IsFlipped,
                    IsMatched = c.IsMatched
                }).ToList()
            };

            string folderPath = "saved_games";
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, $"{CurrentUserName}.json");
            File.WriteAllText(filePath, System.Text.Json.JsonSerializer.Serialize(saveModel, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));
            gameTimer?.Start();
        }

        private void LoadSavedGame()
        {
            string filePath = Path.Combine("saved_games", $"{CurrentUserName}.json");
            if (!File.Exists(filePath))
            {
                MessageBox.Show("No saved game exists for this user.");
                return;
            }

            string json = File.ReadAllText(filePath);
            var saved = System.Text.Json.JsonSerializer.Deserialize<SavedGame>(json);
            if (saved == null)
            {
                MessageBox.Show("The save file is corrupted.");
                return;
            }

            gameEnded = false;
            SelectedCategory = Categories.FirstOrDefault(c => c.Name == saved.CategoryName);
            GridRows = saved.Rows;
            GridCols = saved.Cols;

            totalInitialSeconds = saved.RemainingSeconds;
            stopwatch = System.Diagnostics.Stopwatch.StartNew();

            OnPropertyChanged(nameof(RemainingTime));

            GameCards.Clear();
            foreach (var card in saved.Cards)
            {
                GameCards.Add(new GameCard
                {
                    ImagePath = card.ImagePath,
                    IsFlipped = card.IsFlipped,
                    IsMatched = card.IsMatched
                });
            }

            FirstFlipped = null;
            gameTimer = new System.Windows.Threading.DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromSeconds(1);
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();
        }

        private void UpdateStatistics(bool won)
        {
            var userService = new UserService();
            var user = userService.GetAllUsers().FirstOrDefault(u => u.Name == CurrentUserName);
            if (user != null)
            {
                user.GamesPlayed++;
                if (won)
                    user.GamesWon++;

                userService.SaveAll();
            }
        }

        private void SetGameMode(object param)
        {
            SelectedMode = param?.ToString();
            if (SelectedMode == "Standard")
            {
                GridRows = 4;
                GridCols = 4;
                CustomRows = 4;
                CustomCols = 4;
            }
        }

        private void ExitToLogin()
        {
            var gameWindow = Application.Current.Windows
                .OfType<GameWindow>()
                .FirstOrDefault();

            if (gameWindow != null)
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
                gameWindow.Close();
            }
        }

        private void ShowAbout()
        {
            var about = new AboutWindow();
            about.ShowDialog();
        }

        private void ShowStatistics()
        {
            var statsWindow = new StatisticsWindow();
            statsWindow.ShowDialog();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}