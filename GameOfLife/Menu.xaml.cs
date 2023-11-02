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
using static GameOfLife.Enums;

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

            foreach (string name in Enum.GetNames(typeof(Pattern)))
                InitialShape.Items.Add(name);

            InitialShape.SelectedIndex = (int)Pattern.Empty;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            string boardSizeStr = BoardSize.Text;
            int boardSize;

            if (Int32.TryParse(boardSizeStr, out boardSize))
            {
                Pattern initialPattern = (Pattern)Enum.Parse(typeof(Pattern), InitialShape.SelectedItem.ToString()!);

                Window mainWindow = new MainWindow(boardSize, initialPattern);
                mainWindow.Show();
                this.Close();
            }
            else
                MessageBox.Show("Invalid value for board size!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

        }
    }
}
