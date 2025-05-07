using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace JocMemory.View
{
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        private void Email_Click(object sender, MouseButtonEventArgs e)
        {
            Process.Start(new ProcessStartInfo("lorincz.gabriella@student.unitbv.ro") { UseShellExecute = true });
        }
    }
}
