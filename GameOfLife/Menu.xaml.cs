using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            string boardSizeStr = BoardSize.Text;
            int boardSize;

            if (Int32.TryParse(boardSizeStr, out boardSize))
            {
                string initialShape = ((ComboBoxItem)InitialShape.SelectedItem).Name;

                // to delete after tests
                MessageBox.Show($"Selected values: {boardSize.ToString()}, {initialShape}");

                Window mainWindow = new MainWindow(boardSize, initialShape);
                mainWindow.Show();
                this.Close();
            }
            else
                MessageBox.Show("Invalid value for board size!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

        }
    }
}
