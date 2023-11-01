using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
