using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Core.Entities
{
    public class Map
    {
        public int Width { get; private set; }
        public int Depth { get; private set; }
        public CubeTypes[][] Grid { get; set; }

        public Map(int width, int depth)
        {
            this.Width = width;
            this.Depth = depth;
            Grid = new CubeTypes[width][];
            for (int i = 0; i < width; i++)
            {
                Grid[i] = new CubeTypes[depth];
            }
        }
    }
}
