using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Core.Entities
{
    public class MovingStep
    {
        public int X { get; set; }
        public int Z { get; set; }

        public MovingStep(int x, int z)
        {
            X = x;
            Z = z;
        }
    }
}
