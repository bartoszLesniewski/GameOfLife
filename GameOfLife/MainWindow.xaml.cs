using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int CELL_SIZE = 20;
        private int boardSize;
        private Game game;
        private DispatcherTimer? timer;

        public MainWindow(int boardSize, string initialShape)
        {
            InitializeComponent();

            this.boardSize = boardSize;
            game = new Game(boardSize);
            initializeGrid();
            updateBoardAndStats();
            StopButton.IsEnabled = false;
        }

        private void initializeGrid()
        {
            GameGrid.HorizontalAlignment = HorizontalAlignment.Center;
            GameGrid.VerticalAlignment = VerticalAlignment.Center;

            //GameGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
            //GameGrid.VerticalAlignment = VerticalAlignment.Stretch;

            for (int i = 0; i < boardSize; i++)
            {
                GameGrid.ColumnDefinitions.Add(new ColumnDefinition());
                GameGrid.RowDefinitions.Add(new RowDefinition());
            }
        }

        private void updateBoardAndStats()
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    var cell = new Rectangle
                    {
                        Width = CELL_SIZE,
                        Height = CELL_SIZE,
                        Fill = game.CurrentState.CellsMap[i, j].IsAlive ? Brushes.Black : Brushes.White,
                        Stroke = Brushes.Silver,
                        StrokeThickness = 1,
                    };

                    cell.MouseLeftButtonDown += Rectangle_MouseLeftButtonDown;

                    Grid.SetRow(cell, i);
                    Grid.SetColumn(cell, j);
                    GameGrid.Children.Add(cell);
                }
            }
            Dictionary<string, int> stats = game.GetStatistics();
            GenerationNumber.Text = stats["GenerationNumber"].ToString();
            BornCellsNumber.Text = stats["BornCells"].ToString();
            DeadCellsNumber.Text = stats["DeadCells"].ToString();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            game.NextState();
            updateBoardAndStats();
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            game.PreviousState();
            updateBoardAndStats();
        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            RunButton.IsEnabled = false;
            StopButton.IsEnabled = true;
            timer = new DispatcherTimer();
            timer.Tick += GameTick;
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Start();
        }

        private void GameTick(object? sender, EventArgs e)
        {
            game.NextState();
            updateBoardAndStats();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            timer!.Stop();
            RunButton.IsEnabled = true;
            StopButton.IsEnabled = false;
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            int row = Grid.GetRow(sender as Rectangle);
            int column = Grid.GetColumn(sender as Rectangle);
            game.EditCell(row, column);
            updateBoardAndStats();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
