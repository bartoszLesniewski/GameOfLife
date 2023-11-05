using System;

namespace GameOfLife
{
    public class Cell : ICloneable
    {
        public bool IsAlive { get; set; }

        public Cell (bool isAlive)
        {
            IsAlive = isAlive;
        }

        public object Clone()
        {
            return new Cell(IsAlive);
        }
    }
}
