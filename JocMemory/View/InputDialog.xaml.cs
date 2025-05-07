using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace JocMemory.View
{
    public partial class InputDialog : Window
    {
        public string UserName => UserNameBox.Text;
        private string selectedAvatarPath;

        public string SelectedAvatar => selectedAvatarPath;

        public InputDialog()
        {
            InitializeComponent();
            LoadAvatars();
        }

        private void LoadAvatars()
        {
            string avatarsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Avatars");

            if (Directory.Exists(avatarsFolder))
            {
                foreach (var file in Directory.GetFiles(avatarsFolder)
                         .Where(f => f.EndsWith(".jpg") || f.EndsWith(".png") || f.EndsWith(".gif")))
                {
                    var image = new Image
                    {
                        Source = new BitmapImage(new Uri(file, UriKind.Absolute)),
                        Width = 60,
                        Height = 60,
                        Stretch = Stretch.UniformToFill,
                        Tag = file
                    };

                    var border = new Border
                    {
                        BorderThickness = new Thickness(0),
                        BorderBrush = Brushes.Transparent,
                        Margin = new Thickness(5),
                        Child = image,
                        Cursor = System.Windows.Input.Cursors.Hand
                    };

                    border.MouseLeftButtonDown += AvatarBorder_Click;
                    AvatarWrapPanel.Children.Add(border);
                }
            }
        }

        private void AvatarBorder_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            foreach (Border border in AvatarWrapPanel.Children)
                border.BorderThickness = new Thickness(0);

            var selectedBorder = sender as Border;
            selectedBorder.BorderBrush = Brushes.SlateBlue;
            selectedBorder.BorderThickness = new Thickness(2);

            var selectedImage = selectedBorder.Child as Image;
            selectedAvatarPath = selectedImage.Tag.ToString();
            AvatarPreview.Source = selectedImage.Source;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(selectedAvatarPath))
            {
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Introduceți un nume și alegeți un avatar.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public string GetSelectedAvatarRelativePath()
        {
            if (!string.IsNullOrWhiteSpace(selectedAvatarPath))
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                Uri baseUri = new Uri(basePath.EndsWith("\\") ? basePath : basePath + "\\");
                Uri fileUri = new Uri(selectedAvatarPath);
                return Uri.UnescapeDataString(baseUri.MakeRelativeUri(fileUri).ToString().Replace('/', Path.DirectorySeparatorChar));
            }
            return null;
        }
    }
}
