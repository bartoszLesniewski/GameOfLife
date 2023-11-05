using System;
using static GameOfLife.Enums;

namespace GameOfLife
{
    public static class PatternsGenerator
    {
        public static Cell[,] GeneratePattern(int mapSize, Pattern pattern)
        {
            Cell[,] cellsMap = new Cell[mapSize, mapSize];
            switch (pattern)
            {
                case Pattern.Empty:
                    generateEmptyPattern(cellsMap, mapSize);
                    break;
                case Pattern.Random:
                    generateRandomPattern(cellsMap, mapSize);
                    break;
                case Pattern.Block:
                    generateBlockPattern(cellsMap, mapSize);
                    break;
                case Pattern.Boat:
                    generateBoatPattern(cellsMap, mapSize);
                    break;
                case Pattern.Blinker:
                    generateBlinkerPattern(cellsMap, mapSize);
                    break;
                case Pattern.Beacon:
                    generateBeaconPattern(cellsMap, mapSize);
                    break;
                case Pattern.Gilder:
                    generateGilderPattern(cellsMap, mapSize);
                    break;
                case Pattern.LWSS:
                    generateLWSSPattern(cellsMap, mapSize);
                    break;
                case Pattern.Diehard:
                    generateDiehardPattern(cellsMap, mapSize);
                    break;
                case Pattern.Acorn:
                    generateAcornPattern(cellsMap, mapSize);
                    break;
                default:
                    generateRandomPattern(cellsMap, mapSize);
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

        private static void generateBlockPattern(Cell[,] cellsMap, int mapSize)
        {
            generateEmptyPattern(cellsMap, mapSize);
            cellsMap[1, 1].IsAlive = true;
            cellsMap[1, 2].IsAlive = true;
            cellsMap[2, 1].IsAlive = true;
            cellsMap[2, 2].IsAlive = true;
        }

        private static void generateBoatPattern(Cell[,] cellsMap, int mapSize)
        {
            generateEmptyPattern(cellsMap, mapSize);
            cellsMap[1, 1].IsAlive = true;
            cellsMap[1, 2].IsAlive = true;
            cellsMap[2, 1].IsAlive = true;
            cellsMap[2, 3].IsAlive = true;
            cellsMap[3, 2].IsAlive = true;
        }

        private static void generateBlinkerPattern(Cell[,] cellsMap, int mapSize)
        {
            generateEmptyPattern(cellsMap, mapSize);
            cellsMap[1, 2].IsAlive = true;
            cellsMap[2, 2].IsAlive = true;
            cellsMap[3, 2].IsAlive = true;
        }

        private static void generateBeaconPattern(Cell[,] cellsMap, int mapSize)
        {
            generateEmptyPattern(cellsMap, mapSize);
            cellsMap[1, 1].IsAlive = true;
            cellsMap[1, 2].IsAlive = true;
            cellsMap[2, 1].IsAlive = true;
            cellsMap[2, 2].IsAlive = true;
            cellsMap[3, 3].IsAlive = true;
            cellsMap[3, 4].IsAlive = true;
            cellsMap[4, 3].IsAlive = true;
            cellsMap[4, 4].IsAlive = true;
        }

        private static void generateGilderPattern(Cell[,] cellsMap, int mapSize)
        {
            generateEmptyPattern(cellsMap, mapSize);
            cellsMap[1, 3].IsAlive = true;
            cellsMap[2, 3].IsAlive = true;
            cellsMap[3, 3].IsAlive = true;
            cellsMap[3, 2].IsAlive = true;
            cellsMap[2, 1].IsAlive = true;
        }

        private static void generateLWSSPattern(Cell[,] cellsMap, int mapSize)
        {
            generateEmptyPattern(cellsMap, mapSize);
            cellsMap[2, 3].IsAlive = true;
            cellsMap[2, 4].IsAlive = true;
            cellsMap[2, 5].IsAlive = true;
            cellsMap[2, 6].IsAlive = true;
            cellsMap[3, 2].IsAlive = true;
            cellsMap[5, 2].IsAlive = true;
            cellsMap[3, 6].IsAlive = true;
            cellsMap[4, 6].IsAlive = true;
            cellsMap[5, 5].IsAlive = true;
        }

        private static void generateDiehardPattern(Cell[,] cellsMap, int mapSize)
        {
            generateEmptyPattern(cellsMap, mapSize);
            cellsMap[2, 1].IsAlive = true;
            cellsMap[2, 2].IsAlive = true;
            cellsMap[3, 2].IsAlive = true;
            cellsMap[1, 7].IsAlive = true;
            cellsMap[3, 6].IsAlive = true;
            cellsMap[3, 7].IsAlive = true;
            cellsMap[3, 8].IsAlive = true;
        }

        private static void generateAcornPattern(Cell[,] cellsMap, int mapSize)
        {
            generateEmptyPattern(cellsMap, mapSize);
            cellsMap[1, 2].IsAlive = true;
            cellsMap[3, 1].IsAlive = true;
            cellsMap[3, 2].IsAlive = true;
            cellsMap[2, 4].IsAlive = true;
            cellsMap[3, 5].IsAlive = true;
            cellsMap[3, 6].IsAlive = true;
            cellsMap[3, 7].IsAlive = true;
        }
    }
}
