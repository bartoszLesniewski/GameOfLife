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

namespace GameOfLife
{
    public class GameState : ICloneable
    {
        public Cell[,] cellsMap { get; private set; }
        public int mapSize { get; private set; }

        public GameState(int mapSize, bool random)
        {
            this.mapSize = mapSize;
            cellsMap = new Cell[mapSize, mapSize];
            generateRandomState();
        }

        public GameState(int mapSize, Cell[,] cellsMap)
        {
            this.mapSize = mapSize;
            this.cellsMap = cellsMap;
        }

        public object Clone()
        {
            var clonedCellsMap = new Cell[mapSize, mapSize];
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    clonedCellsMap[i, j] = (Cell)cellsMap[i, j].Clone();
                }
            }

            return new GameState(mapSize, clonedCellsMap);
        }

        private void generateRandomState()
        {
            Random rnd = new Random();

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    cellsMap[i, j] = new Cell(rnd.Next(5) == 0);
                    //cellsMap[i, j] = new Cell(false);
                }
            }

            cellsMap[5, 4].IsAlive = true;
            cellsMap[5, 6].IsAlive = true;
            cellsMap[4, 5].IsAlive = true;
            cellsMap[3, 6].IsAlive = true;
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

                    if (neighbourRow >= 0 && neighbourRow < mapSize && neighbourCol >= 0 && neighbourCol < mapSize)
                    {
                        if (cellsMap[neighbourRow, neighbourCol].IsAlive && cellsMap[neighbourRow, neighbourCol] != cellsMap[row, col])
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
        private List<Cell> getChangedCells()
        {
            List<Cell> changedCells = new List<Cell>();

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    int liveNeighbours = getNumberOfLiveNeighbours(i, j);
                    Cell currentCell = cellsMap[i, j];

                    if (currentCell.IsAlive && (liveNeighbours < 2 || liveNeighbours > 3))
                        changedCells.Add(currentCell);
                    else if (!currentCell.IsAlive && liveNeighbours == 3)
                        changedCells.Add(currentCell);
                }
            }

            return changedCells;
        }

        public void UpdateCells()
        {
            var changedCells = getChangedCells();

            foreach(var cell in changedCells)
            {
                cell.IsAlive = !cell.IsAlive;
            }
        }

        public GameState GetNextState()
        {
            GameState nextState = (GameState)this.Clone();
            nextState.UpdateCells();

            return nextState;
        }

        public void ChangeCellState(int row, int column)
        {
            cellsMap[row, column].IsAlive = !cellsMap[row, column].IsAlive;
        }
    }
}
