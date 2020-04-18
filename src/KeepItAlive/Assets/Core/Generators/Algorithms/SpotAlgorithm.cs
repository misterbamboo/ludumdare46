using Assets.Core.Entities;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Core.Generators.Algorithms
{
    public class SpotAlgorithm
    {
        public int SpotMaxRadius { get; set; } = 25;
        public int SpotMinRadius { get; set; } = 5;

        public SpotAlgorithm(Map map, CubeTypes cubeType, CubeTypes paddingCubeType)
        {
            Map = map;
            SpotType = cubeType;
            PaddingType = paddingCubeType;
        }

        private CubeTypes SpotType { get; set; }
        private CubeTypes PaddingType { get; set; }
        private Map Map { get; set; }

        public void GenerateSpot()
        {
            int maxTentatives = 100;
            do
            {
                var x = Random.Range(0, Map.Width);
                var z = Random.Range(0, Map.Depth);

                if (Map.IsCubeType(x, z, CubeTypes.Empty))
                {
                    FoundSpotGeneration(x, z);
                    break;
                }
            } while (maxTentatives-- > 0);
        }

        private void FoundSpotGeneration(int x, int z)
        {
            var radius = Random.Range(SpotMinRadius, SpotMaxRadius);
            Map.PlaceCube(SpotType, x, z);

            // Make a 360 around this spot to fill cubes
            for (int a = 0; a < 360; a++)
            {
                var radiusIrregularities = Random.Range(Constants.MinRadiusIrregularities, Constants.MaxRadiusIrregularities);
                var alteredRadius = radius + radiusIrregularities;

                // for each angles, scan from 1 to radius to add blocks
                for (int r = 1; r <= alteredRadius; r++)
                {
                    int calculatedX = (int)(Mathf.Cos(a) * r) + x;
                    int calculatedZ = (int)(Mathf.Sin(a) * r) + z;

                    if (Map.IsCubeType(calculatedX, calculatedZ, CubeTypes.Empty))
                    {
                        var cubeType = SpotType;
                        if (r == alteredRadius)
                        {
                            cubeType = PaddingType;
                        }
                        
                        Map.PlaceCube(cubeType, calculatedX, calculatedZ);
                    }
                }
            }
        }

        private class Constants
        {
            public const int MinRadiusIrregularities = -2;
            public const int MaxRadiusIrregularities = 3; // max is excluded value
        }
    }
}