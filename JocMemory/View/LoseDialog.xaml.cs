using System.Windows;

namespace JocMemory.View
{
    public partial class LoseDialog : Window
    {
        public LoseDialog()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
