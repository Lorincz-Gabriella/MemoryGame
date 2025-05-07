using System;
using System.Windows;

namespace JocMemory.View
{
    public partial class TimeInputDialog : Window
    {
        public int Minutes { get; private set; }

        public TimeInputDialog()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(TimeBox.Text, out int min) && min > 0)
            {
                Minutes = min;
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Introduceți un număr valid de minute (>0).", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
