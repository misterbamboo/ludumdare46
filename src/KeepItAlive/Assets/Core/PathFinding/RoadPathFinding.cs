using Assets.Core.DI;
using Assets.Core.Entities;
using Assets.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Core.PathFinding
{
    public class RoadPathFinding
    {
        private IMapService MapService => DependencyInjection.Get<IMapService>();

        public IEnumerable<MovingStep> FromTo(int startX, int startZ)
        {
            var cubeType = MapService.GetCubeType(startX, startZ);
            if (cubeType != CubeTypes.Road) return Enumerable.Empty<MovingStep>();

            List<IEnumerable<MovingStep>> roads = new List<IEnumerable<MovingStep>>();
            for (int xoff = -1; xoff <= 1; xoff++)
            {
                if (xoff == 0) continue;
                IEnumerable<MovingStep> road = FollowRoad(startX + xoff, startZ);
                roads.Add(road);
            }

            for (int zoff = -1; zoff <= 1; zoff++)
            {
                if (zoff == 0) continue;
                IEnumerable<MovingStep> road = FollowRoad(startX, startZ + zoff);
                roads.Add(road);
            }

            IEnumerable<MovingStep> bestRoad = null;
            int bestRoadZ = 0;
            foreach (var road in roads)
            {
                int z = ZMostRoadPoint(road);
                if (z >= bestRoadZ)
                {
                    bestRoadZ = z;
                    bestRoad = road;
                }
            }

            if (bestRoad == null)
            {
                return Enumerable.Empty<MovingStep>();
            }
            return bestRoad;
        }

        private int ZMostRoadPoint(IEnumerable<MovingStep> roadSteps)
        {
            int zEst = 0;
            foreach (var roadStep in roadSteps)
            {
                if (roadStep.Z > zEst)
                {
                    zEst = roadStep.Z;
                }
            }
            return zEst;
        }

        private IEnumerable<MovingStep> FollowRoad(int x, int z)
        {
            var cubeType = MapService.GetCubeType(x, z);
            if (cubeType != CubeTypes.Road) return Enumerable.Empty<MovingStep>();

            var fullRoad = new List<MovingStep>() { new MovingStep(x, z) };

            // check foward
            IEnumerable<MovingStep> road = FollowRoad(x, z + 1);
            if (road.Any())
            {
                fullRoad.AddRange(road);
                return fullRoad;
            }

            for (int xoff = -1; xoff <= 1; xoff++)
            {
                if (xoff == 0) continue;
                IEnumerable<MovingStep> sideRoad = FollowRoad(x + xoff, z);
                if (sideRoad.Any())
                {
                    fullRoad.AddRange(sideRoad);
                    return fullRoad;
                }
            }

            return fullRoad;
        }
    }
}
