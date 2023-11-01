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
        public Game(int boardSize) 
        {
            CurrentState = new GameState(boardSize, random: true);
            states = new List<GameState>
            {
                CurrentState
            };
        }

        public void NextState()
        {
            if (CurrentState == states.Last())
            {
                CurrentState = CurrentState.GetNextState();
                states.Add(CurrentState);
            }
            else
            {
                int currentStateIdx = states.IndexOf(CurrentState);
                CurrentState = states[currentStateIdx + 1];
            }
        }

        public void PreviousState()
        {
            int currentStateIdx = states.IndexOf(CurrentState);
            if (currentStateIdx - 1 >= 0)
            {
                CurrentState = states[currentStateIdx - 1];
            }
            else
            {
                MessageBox.Show("This is the first state!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
