using System.Collections.ObjectModel;
using System.IO;
using JocMemory.Model;
using System.Text.Json;

namespace JocMemory.Services
{
    public class UserService
    {
        private const string FileName = "usersjson"; 
        private ObservableCollection<User> users;

        public UserService()
        {
            users = LoadUsers(); 
        }

        public ObservableCollection<User> GetAllUsers() => users;

        public void AddUser(string name, string imagePath)
        {
            users.Add(new User { Name = name, ImagePath = imagePath });
            SaveUsers();
        }

        public void RemoveUser(User user)
        {
            if (users.Contains(user))
            {
                users.Remove(user);
                string savedGamePath = Path.Combine("saved_games", $"{user.Name}.json");
                if (File.Exists(savedGamePath))
                {
                    File.Delete(savedGamePath);
                }
                SaveUsers();
            }
        }

        private void SaveUsers()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(users, options);
            File.WriteAllText(FileName, json);
        }

        private ObservableCollection<User> LoadUsers()
        {
            if (File.Exists(FileName))
            {
                string json = File.ReadAllText(FileName);
                return JsonSerializer.Deserialize<ObservableCollection<User>>(json);
            }

            return new ObservableCollection<User>();
        }

        public void SaveAll()
        {
            SaveUsers();
        }

    }
}
