using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using JocMemory.Model;
using JocMemory.Services;
using JocMemory.View;

namespace JocMemory.ViewModel
{
    public class MeniuCommand : INotifyPropertyChanged
    {
        private readonly UserService userService;
        public string BackgroundImagePath => "Images/b2.jpg";

        public ObservableCollection<User> Users { get; set; }

        public ICommand AddUserCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand DeleteUserCommand => deleteUserCommand;
        public ICommand PlayCommand => playCommand;


        private RelayCommand deleteUserCommand;
        private RelayCommand playCommand;

        private User selectedUser;
        public User SelectedUser
        {
            get => selectedUser;
            set
            {
                selectedUser = value;
                OnPropertyChanged();
                deleteUserCommand.RaiseCanExecuteChanged();
                playCommand.RaiseCanExecuteChanged();
            }
        }

        public MeniuCommand()
        {
            userService = new UserService();
            Users = userService.GetAllUsers();

            AddUserCommand = new RelayCommand(_ => AddUser());
            deleteUserCommand = new RelayCommand(_ => DeleteUser(), _ => SelectedUser != null);
            playCommand = new RelayCommand(_ => PlayGame(), _ => SelectedUser != null);
            ExitCommand = new RelayCommand(_ => Application.Current.Shutdown());
        }

        private void AddUser()
        {
            var dialog = new InputDialog();
            if (dialog.ShowDialog() == true)
            {
                string name = dialog.UserName.Trim();
                string relativePath = dialog.GetSelectedAvatarRelativePath();

                if (!string.IsNullOrEmpty(name) && !name.Contains(" "))
                {
                    userService.AddUser(name, relativePath);
                }
                else
                {
                    MessageBox.Show("The name must contain only one word!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void DeleteUser()
        {
            if (SelectedUser != null)
            {
                userService.RemoveUser(SelectedUser);
                SelectedUser = null;
            }
        }

        private void PlayGame()
        {
            GameWindow gameWindow = new GameWindow();

            if (gameWindow.DataContext is GameViewModel vm && SelectedUser != null)
            {
                vm.CurrentUserName = SelectedUser.Name;
            }

            gameWindow.Show();

            Application.Current.Windows
                       .OfType<MainWindow>()
                       .FirstOrDefault()
                       ?.Close();
                       }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
