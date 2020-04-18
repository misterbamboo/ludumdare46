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
            int startingX = Map.Width / Constants.Half;
            for (int i = 0; i < Map.Depth; i++)
            {
                startingX += Random.Range(Constants.MinusOneInclusive, Constants.PlusOneInclusive);
                startingX = Mathf.Clamp(startingX, 0, Map.Width);

                size += Random.Range(Constants.MinusOneInclusive, Constants.PlusOneInclusive);
                size = Mathf.Clamp(size, MinBoldSize, MaxBoldSize);

                int beginX = startingX - size / 2;
                int endX = beginX + size;

                beginX = Mathf.Clamp(beginX, 0, Map.Width);
                endX = Mathf.Clamp(endX, 0, Map.Width);

                for (int x = beginX; x < endX; x++)
                {
                    Map.Grid[x][i] = CubeTypes.Grass;
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