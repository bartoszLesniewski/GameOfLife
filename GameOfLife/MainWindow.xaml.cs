using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using static GameOfLife.Enums;

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

        public MainWindow(int boardSize, Pattern initialPattern, int minNeighbours, int maxNeighbours)
        {
            InitializeComponent();

            this.boardSize = boardSize;
            game = new Game(boardSize, initialPattern, minNeighbours, maxNeighbours);
            initializeGrid();
            updateBoardAndStats();
            StopButton.IsEnabled = false;
        }

        private void initializeGrid()
        {
            GameGrid.Children.Clear();
            GameGrid.RowDefinitions.Clear();
            GameGrid.ColumnDefinitions.Clear();
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
                    Brush brushAlive = Brushes.Black;
                    Brush brushDead = Brushes.White;

                    if ((bool)HighlightCheck.IsChecked! &&
                        game.CurrentState.ChangedCells.Contains(game.CurrentState.CellsMap[i, j]))
                    {
                        brushAlive = Brushes.Green;
                        brushDead = Brushes.Red;
                    }

                    var cell = new Rectangle
                    {
                        Width = CELL_SIZE,
                        Height = CELL_SIZE,
                        Fill = game.CurrentState.CellsMap[i, j].IsAlive ? brushAlive : brushDead,
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
            UpdateLayout();
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
            string fileContent = game.GetCurrentStateInFormatToSave();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "game_of_life_state_save";
            saveFileDialog.DefaultExt = ".txt";
            saveFileDialog.Filter = "Text documents (.txt)|*.txt";

            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, fileContent);
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == true)
            {
                string fileContent = File.ReadAllText(openFileDialog.FileName);
                game = new Game(fileContent);
                boardSize = game.BoardSize;
                initializeGrid();
                updateBoardAndStats();
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            game = new Game(boardSize, Pattern.Empty, 
                game.CurrentState.MinNumberOfNeighbours, 
                game.CurrentState.MaxNumberOfNeighbours);
            updateBoardAndStats();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Window menuWindow = new Menu();
            menuWindow.Show();
            this.Close();
        }
    }
}
