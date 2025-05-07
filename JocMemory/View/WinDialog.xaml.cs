using System.Windows;

namespace JocMemory.View
{
    public partial class WinDialog : Window
    {
        public WinDialog()
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
