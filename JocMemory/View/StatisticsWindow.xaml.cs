using System.Windows;
using JocMemory.Services;
using JocMemory.Model;
using System.Collections.ObjectModel;

namespace JocMemory.View
{
    public partial class StatisticsWindow : Window
    {
        public ObservableCollection<User> Users { get; set; }

        public StatisticsWindow()
        {
            InitializeComponent();
            var service = new UserService();
            Users = service.GetAllUsers();
            DataContext = this;
        }
    }
}
