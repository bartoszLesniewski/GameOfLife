﻿using System;
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
        public Cell[,] CellsMap { get; private set; }
        public int MapSize { get; private set; }
        public Dictionary<string, int> Statistics { get; private set; }

        public GameState(int mapSize, bool random)
        {
            MapSize = mapSize;
            CellsMap = new Cell[mapSize, mapSize];
            Statistics = new Dictionary<string, int>();
            Statistics["GenerationNumber"] = 0;
            Statistics["BornCells"] = 0;
            Statistics["DeadCells"] = 0;
            generateRandomState();
        }

        public GameState(int mapSize, Cell[,] cellsMap, Dictionary<string, int> statistics)
        {
            MapSize = mapSize;
            CellsMap = cellsMap;
            Statistics = statistics;
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

            return new GameState(MapSize, clonedCellsMap, clonedStatistics);
        }

        private void generateRandomState()
        {
            Random rnd = new Random();

            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    CellsMap[i, j] = new Cell(rnd.Next(5) == 0);
                    //cellsMap[i, j] = new Cell(false);
                }
            }

            CellsMap[5, 4].IsAlive = true;
            CellsMap[5, 6].IsAlive = true;
            CellsMap[4, 5].IsAlive = true;
            CellsMap[3, 6].IsAlive = true;
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
        private List<Cell> getChangedCells()
        {
            List<Cell> changedCells = new List<Cell>();

            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    int liveNeighbours = getNumberOfLiveNeighbours(i, j);
                    Cell currentCell = CellsMap[i, j];

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
    }
}