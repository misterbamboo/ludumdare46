using Assets.Core.Entities;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Core.Generators.Algorithms
{
    public class GrassPathAlgorithm
    {
        public int StartingBoldSize { get; set; } = 3;
        public int MaxBoldSize { get; set; } = 8;
        public int MinBoldSize { get; set; } = 2;

        public GrassPathAlgorithm(Map map)
        {
            Map = map;
        }

        private Map Map { get; set; }

        public void GeneratePath()
        {
            int size = StartingBoldSize;
            int pointerX = Map.Width / Constants.Half;
            for (int z = 0; z < Map.Depth; z++)
            {
                pointerX += Random.Range(Constants.MinusOneInclusive, Constants.PlusOneInclusive);
                pointerX = Mathf.Clamp(pointerX, 0, Map.Width);

                size += Random.Range(Constants.MinusOneInclusive, Constants.PlusOneInclusive);
                size = Mathf.Clamp(size, MinBoldSize, MaxBoldSize);

                int beginX = pointerX - size / 2;
                int endX = beginX + size;
                for (int x = beginX; x < endX; x++)
                {
                    Map.PlaceCube(CubeTypes.Grass, x, z);
                }
            }
        }

        private class Constants
        {
            public const int MinusOneInclusive = -1;
            public const int PlusOneInclusive = 2; // Random excude end number
            public const int Half = 2;
        }
    }
}