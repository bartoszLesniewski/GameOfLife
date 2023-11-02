using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Shapes;
using static GameOfLife.Enums;

namespace GameOfLife
{
    public class GameState : ICloneable
    {
        public Cell[,] CellsMap { get; private set; }
        public int MapSize { get; private set; }
        public Dictionary<string, int> Statistics { get; private set; }

        // this is a required number of neighbours for birth of a new cell
        // and also the maximum number of neighbours that living cell can have
        // (above this number, a living cell dies)
        public int MaxNumberOfNeighbours { get; private set; }

        // this is a minimum number of neighbours that living cell can have
        // (below this number, a living cell dies)
        public int MinNumberOfNeighbours { get; private set; }
        public List<Cell> ChangedCells { get; private set; }

        public GameState(int mapSize, Pattern pattern, int minNumberOfNeighbours, int maxNumberOfNeighbours)
        {
            MapSize = mapSize;
            CellsMap = PatternsGenerator.GeneratePattern(mapSize, pattern);
            Statistics = new Dictionary<string, int>();
            Statistics["GenerationNumber"] = 0;
            Statistics["BornCells"] = 0;
            Statistics["DeadCells"] = 0;
            MinNumberOfNeighbours = minNumberOfNeighbours;
            MaxNumberOfNeighbours = maxNumberOfNeighbours;
            ChangedCells = new List<Cell>();
        }

        public GameState(int mapSize, Cell[,] cellsMap, Dictionary<string, int> statistics, 
            int minNumberOfNeighbours, int maxNumberOfNeighbours)
        {
            MapSize = mapSize;
            CellsMap = cellsMap;
            Statistics = statistics;
            MinNumberOfNeighbours = minNumberOfNeighbours;
            MaxNumberOfNeighbours = maxNumberOfNeighbours;
            ChangedCells = new List<Cell>();
        }

        public GameState(string stateFromFile)
        {
            string[] lines = stateFromFile.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            string[] initData = lines[0].Split(";");

            Statistics = new Dictionary<string, int>();
            MapSize = Int32.Parse(initData[0]);
            MinNumberOfNeighbours = Int32.Parse(initData[1]);
            MaxNumberOfNeighbours = Int32.Parse(initData[2]);
            Statistics["GenerationNumber"] = Int32.Parse(initData[3]);
            Statistics["BornCells"] = Int32.Parse(initData[4]);
            Statistics["DeadCells"] = Int32.Parse(initData[5]);
            CellsMap = new Cell[MapSize, MapSize];

            for (int i = 1; i < lines.Length; i++)
            {
                string[] cellData = lines[i].Split(";");

                int row = Int32.Parse(cellData[0]);
                int column = Int32.Parse(cellData[1]);
                bool state = Int32.Parse(cellData[2]) == 1 ? true : false;
                CellsMap[row, column] = new Cell(state);
            }

            ChangedCells = new List<Cell>();
        }

        public object Clone()
        {
            var clonedCellsMap = new Cell[MapSize, MapSize];
            var clonedStatistics = new Dictionary<string, int>(Statistics);

            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    clonedCellsMap[i, j] = (Cell)CellsMap[i, j].Clone();
                }
            }

            return new GameState(MapSize, clonedCellsMap, clonedStatistics, MinNumberOfNeighbours, MaxNumberOfNeighbours);
        }

        private int getNumberOfLiveNeighbours(int row, int col)
        {
            int neighbours = 0;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int neighbourRow = row + i;
                    int neighbourCol = col + j;

                    if (neighbourRow >= 0 && neighbourRow < MapSize && neighbourCol >= 0 && neighbourCol < MapSize)
                    {
                        if (CellsMap[neighbourRow, neighbourCol].IsAlive && CellsMap[neighbourRow, neighbourCol] != CellsMap[row, col])
                            neighbours++;
                    }
                }
            }
            return neighbours;
        }

        /*
        1. Any live cell with fewer than two live neighbours dies, as if by underpopulation.
        2. Any live cell with two or three live neighbours lives on to the next generation.
        3. Any live cell with more than three live neighbours dies, as if by overpopulation.
        4. Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
        */    
        private void getChangedCells()
        {
            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    int liveNeighbours = getNumberOfLiveNeighbours(i, j);
                    Cell currentCell = CellsMap[i, j];

                    if (currentCell.IsAlive && (liveNeighbours < MinNumberOfNeighbours || liveNeighbours > MaxNumberOfNeighbours))
                        ChangedCells.Add(currentCell);
                    else if (!currentCell.IsAlive && liveNeighbours == MaxNumberOfNeighbours)
                        ChangedCells.Add(currentCell);
                }
            }
        }

        public void UpdateCells()
        {
            getChangedCells();

            foreach(var cell in ChangedCells)
            {
                if (cell.IsAlive)
                    Statistics["DeadCells"]++;
                else
                    Statistics["BornCells"]++;

                cell.IsAlive = !cell.IsAlive;
            }

            Statistics["GenerationNumber"]++;
        }

        public GameState GetNextState()
        {
            GameState nextState = (GameState)this.Clone();
            nextState.UpdateCells();

            return nextState;
        }

        public void ChangeCellState(int row, int column)
        {
            CellsMap[row, column].IsAlive = !CellsMap[row, column].IsAlive;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{MapSize};{MinNumberOfNeighbours};{MaxNumberOfNeighbours};" +
                $"{Statistics["GenerationNumber"]};{Statistics["BornCells"]};{Statistics["DeadCells"]}");
            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    sb.AppendLine($"{i};{j};{(CellsMap[i, j].IsAlive ? 1 : 0)}");
                }
            }

            return sb.ToString();
        }
    }
}
