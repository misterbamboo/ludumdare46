using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Core.Entities
{
    public class Map
    {
        public int Width { get; private set; }
        public int Depth { get; private set; }

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

        private CubeTypes[][] Grid { get; set; }

        public void FillEmptySpacesWith(CubeTypes cubeType)
        {
            for (int x = 0; x < Grid.Length; x++)
            {
                for (int z = 0; z < Grid[x].Length; z++)
                {
                    if (Grid[x][z] == CubeTypes.Empty)
                    {
                        Grid[x][z] = cubeType;
                    }
                }
            }
        }

        public void PlaceCube(CubeTypes cubeType, int x, int z)
        {
            x = Mathf.Clamp(x, 0, Width - 1);
            z = Mathf.Clamp(z, 0, Depth - 1);
            Grid[x][z] = cubeType;
        }

        public CubeTypes GetCubeType(int x, int z)
        {
            if (x < 0 || x >= Width) return CubeTypes.Empty;
            if (z < 0 || z >= Depth) return CubeTypes.Empty;

            return Grid[x][z];
        }

        public bool IsCubeType(int x, int z, CubeTypes cubeType)
        {
            if (x < 0 || x >= Width) return false;
            if (z < 0 || z >= Depth) return false;

            return Grid[x][z] == cubeType;
        }
    }
}
