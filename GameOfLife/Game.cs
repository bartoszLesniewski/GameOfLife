using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameOfLife
{
    public class Game
    {
        public GameState CurrentState { get; private set; }
        private List<GameState> states;
        public int BoardSize { get; private set; }

        public Game(int boardSize) 
        {
            CurrentState = new GameState(boardSize, random: true);
            states = new List<GameState>();
            BoardSize = boardSize;
        }

        public Game(string stateFromFile)
        {
            CurrentState = new GameState(stateFromFile);
            states = new List<GameState>();
            BoardSize = CurrentState.MapSize;
        }

        public void NextState()
        {
            states.Add(CurrentState);
            CurrentState = CurrentState.GetNextState();
        }

        public void PreviousState()
        {
            if (states.Count > 0)
            {
                CurrentState = states.Last();
                states.Remove(CurrentState);
            }
            else
            {
                MessageBox.Show("This is the first state!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void EditCell(int row, int column)
        {
            CurrentState.ChangeCellState(row, column);
        }

        public Dictionary<string, int> GetStatistics()
        {
            return CurrentState.Statistics;
        }

        public string GetCurrentStateInFormatToSave()
        {
            return CurrentState.ToString();
        }
    }
}
