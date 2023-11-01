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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int BOARD_SIZE = 20;
        const int CELL_SIZE = 20;
        private Game game;
        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();

            game = new Game(BOARD_SIZE);
            initializeGrid();
            updateBoard();
        }

        private void initializeGrid()
        {
            GameGrid.HorizontalAlignment = HorizontalAlignment.Center;
            GameGrid.VerticalAlignment = VerticalAlignment.Center;

            //GameGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
            //GameGrid.VerticalAlignment = VerticalAlignment.Stretch;

            for (int i = 0; i < BOARD_SIZE; i++)
            {
                GameGrid.ColumnDefinitions.Add(new ColumnDefinition());
                GameGrid.RowDefinitions.Add(new RowDefinition());
            }
        }

        private void updateBoard()
        {
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    var cell = new Rectangle
                    {
                        Width = CELL_SIZE,
                        Height = CELL_SIZE,
                        Fill = game.CurrentState.cellsMap[i, j].IsAlive ? Brushes.Black : Brushes.White,
                        Stroke = Brushes.Silver,
                        StrokeThickness = 1,
                    };

                    cell.MouseLeftButtonDown += Rectangle_MouseLeftButtonDown;

                    Grid.SetRow(cell, i);
                    Grid.SetColumn(cell, j);
                    GameGrid.Children.Add(cell);
                }
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            game.NextState();
            updateBoard();
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            game.PreviousState();
            updateBoard();
        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            RunButton.IsEnabled = false;
            timer = new DispatcherTimer();
            timer.Tick += GameTick;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();
        }

        private void GameTick(object? sender, EventArgs e)
        {
            game.NextState();
            updateBoard();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            RunButton.IsEnabled = true;
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            int row = Grid.GetRow(sender as Rectangle);
            int column = Grid.GetColumn(sender as Rectangle);
            MessageBox.Show($"Rectangle clicked! {row} {column}", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
