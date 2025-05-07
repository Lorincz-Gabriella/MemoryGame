using System.Windows;

namespace JocMemory.View
{
    public partial class CustomSizeDialog : Window
    {
        public int Rows { get; private set; }
        public int Cols { get; private set; }

        public CustomSizeDialog()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(RowsBox.Text, out int rows) && int.TryParse(ColsBox.Text, out int cols) &&
                rows >= 2 && rows <= 6 && cols >= 2 && cols <= 6 && (rows * cols) % 2 == 0)
            {
                Rows = rows;
                Cols = cols;
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Introduceți valori între 2 și 6 și un număr par total de cărți!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
