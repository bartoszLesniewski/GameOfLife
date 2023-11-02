using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GameOfLife.Enums;

namespace GameOfLife
{
    public static class PatternsGenerator
    {
        public static Cell[,] GeneratePattern(int mapSize, Pattern pattern)
        {
            Cell[,] cellsMap = new Cell[mapSize, mapSize];
            switch(pattern)
            {
                case Pattern.Random:
                    generateRandomPattern(cellsMap, mapSize);
                    break;
                case Pattern.Empty:
                    generateEmptyPattern(cellsMap, mapSize);
                    break;
                default:
                    generateRandomPattern(cellsMap , mapSize);
                    break;

            }

            return cellsMap;
        }

        private static void generateRandomPattern(Cell[,] cellsMap, int mapSize)
        {
            Random rnd = new Random();

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    cellsMap[i, j] = new Cell(rnd.Next(5) == 0);
                }
            }
        }

        private static void generateEmptyPattern(Cell[,] cellsMap, int mapSize)
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    cellsMap[i, j] = new Cell(false);
                }
            }
        }
    }
}
