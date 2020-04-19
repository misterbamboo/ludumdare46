using Assets.Core.DI;
using Assets.Core.Entities;
using Assets.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Core.PathFinding
{
    public class AstarPathFinding
    {
        private IMapService MapService => DependencyInjection.Get<IMapService>();

        private Dictionary<KeyValuePair<int, int>, AstarDecoratorValues> AllPossibilities { get; set; }

        public CubeTypes? LimitCubeType { get; set; }

        public bool NoDiagonals { get; set; }

        public IEnumerable<MovingStep> FromTo(int startX, int startZ, int destinationX, int destinationZ)
        {
            var blockCount = MapService.GetTotalBocks();
            AllPossibilities = new Dictionary<KeyValuePair<int, int>, AstarDecoratorValues>(blockCount);

            var startingPosition = new AstarDecoratorValues(new MovingStep(startX, startZ), null);
            CalculateCost(startX, startZ, destinationX, destinationZ, startingPosition);

            var foundPath = ContinuePathFinding(startingPosition, startX, startZ, destinationX, destinationZ);
            while (foundPath == null)
            {
                foundPath = GetNextPossibility();
                if (foundPath == null)
                {
                    break;
                }
                else if (foundPath.HCost == 0)
                {
                    break;
                }

                foundPath.AlreadyChecked = true;
                var newFound = ContinuePathFinding(foundPath, startX, startZ, destinationX, destinationZ);
                if (newFound != null)
                {
                    foundPath = newFound;
                    break;
                }
                foundPath = null;
            }

            if (foundPath == null) return Enumerable.Empty<MovingStep>();

            var stepsToDestination = new Stack<MovingStep>();
            var currentPath = foundPath;

            while (currentPath.Parent != null)
            {
                stepsToDestination.Push(currentPath.PossibleStep);
                currentPath = currentPath.Parent;
            }

            return stepsToDestination.ToList();
        }

        private AstarDecoratorValues GetBestPossibility(AstarDecoratorValues currentPath)
        {
            AstarDecoratorValues actualBest = null;
            for (int x = -1; x <= 1; x++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    if (x == 0 && z == 0) continue;
                    var possibility = GetPossibilityAt(currentPath.PossibleStep.X + x, currentPath.PossibleStep.Z + z);
                    if (possibility == null) continue;

                    if (possibility.GCost == 0) return possibility;

                    if (actualBest == null || possibility.FCost < actualBest.FCost)
                    {
                        actualBest = possibility;
                    }
                }
            }
            return actualBest;
        }

        private AstarDecoratorValues GetPossibilityAt(int x, int z)
        {
            KeyValuePair<int, int> key = new KeyValuePair<int, int>(x, z);
            if (!AllPossibilities.ContainsKey(key))
            {
                return null;
            }
            return AllPossibilities[key];
        }

        private void CalculateCost(int startX, int startZ, int destinationX, int destinationZ, AstarDecoratorValues aStarValue)
        {
            aStarValue.GCost = CalculateCostRecursive(aStarValue.PossibleStep.X, aStarValue.PossibleStep.Z, startX, startZ);
            aStarValue.HCost = CalculateCostRecursive(aStarValue.PossibleStep.X, aStarValue.PossibleStep.Z, destinationX, destinationZ);
        }

        private AstarDecoratorValues ContinuePathFinding(AstarDecoratorValues currentPosition, int startX, int startZ, int destinationX, int destinationZ)
        {
            IEnumerable<AstarDecoratorValues> astarSteps = DiscoverPossibleStepsArround(currentPosition, startX, startZ, destinationX, destinationZ);
            foreach (var astarStep in astarSteps)
            {
                var similarPossibility = GetPossibilityAt(astarStep.PossibleStep.X, astarStep.PossibleStep.Z);

                KeyValuePair<int, int> key = new KeyValuePair<int, int>(astarStep.PossibleStep.X, astarStep.PossibleStep.Z);

                if (AllPossibilities.ContainsKey(key))
                {
                    var older = AllPossibilities[key];
                    if (older.FCost > astarStep.FCost)
                    {
                        AllPossibilities[key] = astarStep;
                    }
                }
                else
                {
                    AllPossibilities[key] = astarStep;
                }

                if (similarPossibility != null && similarPossibility.AlreadyChecked)
                {
                    astarStep.AlreadyChecked = similarPossibility.AlreadyChecked;
                }
            }

            AstarDecoratorValues nextPossibility = GetNextPossibility();
            if (nextPossibility != null)
            {
                if (nextPossibility.HCost == 0)
                {
                    return nextPossibility;
                }
            }
            return null;
        }

        private AstarDecoratorValues GetNextPossibility()
        {
            return AllPossibilities.Values.Where(p => !p.AlreadyChecked).OrderBy(p => p.FCost).ThenBy(p => p.HCost).FirstOrDefault();
        }

        private int CalculateCostRecursive(int x1, int z1, int x2, int z2)
        {
            var xdiff = Mathf.Clamp(x2 - x1, -1, 1);
            var zdiff = Mathf.Clamp(z2 - z1, -1, 1);

            var sum = Math.Abs(xdiff) + Math.Abs(zdiff);
            int cost = 0;
            if (sum >= 2)
            {
                cost = 14;
            }
            else if (sum == 1)
            {
                cost = 10;
            }
            else
            {
                return cost;
            }

            int subCost = CalculateCostRecursive(x1 + xdiff, z1 + zdiff, x2, z2);
            return subCost + cost;
        }

        private IEnumerable<AstarDecoratorValues> DiscoverPossibleStepsArround(AstarDecoratorValues currentPosition, int startX, int startZ, int destinationX, int destinationZ)
        {
            List<AstarDecoratorValues> possibleSteps = new List<AstarDecoratorValues>();
            for (int xOff = -1; xOff <= 1; xOff++)
            {
                for (int zOff = -1; zOff <= 1; zOff++)
                {
                    var lookedX = currentPosition.PossibleStep.X + xOff;
                    var lookedZ = currentPosition.PossibleStep.Z + zOff;

                    if (xOff == 0 && zOff == 0)
                    {
                        continue;
                    }

                    if (NoDiagonals)
                    {
                        if (xOff == -1 && zOff == -1) continue;
                        if (xOff == -1 && zOff == 1) continue;
                        if (xOff == 1 && zOff == -1) continue;
                        if (xOff == 1 && zOff == 1) continue;
                    }

                    if (!MapService.IsBlockedPosition(lookedX, lookedZ))
                    {
                        if (LimitCubeType.HasValue && MapService.GetCubeType(lookedX, lookedZ) != LimitCubeType.Value)
                        {
                            continue;
                        }

                        var possibleStep = new MovingStep(lookedX, lookedZ);
                        var aStar = new AstarDecoratorValues(possibleStep, currentPosition);
                        CalculateCost(startX, startZ, destinationX, destinationZ, aStar);

                        // found destination
                        if (aStar.HCost == 0)
                        {
                            return new AstarDecoratorValues[] { aStar };
                        }

                        possibleSteps.Add(aStar);
                    }
                }
            }
            return possibleSteps;
        }

        private class AstarDecoratorValues
        {
            public AstarDecoratorValues Parent { get; private set; }
            public MovingStep PossibleStep { get; set; }
            public int GCost { get; set; }
            public int HCost { get; set; }
            public bool AlreadyChecked { get; set; }
            public int FCost => GCost + HCost;

            public AstarDecoratorValues(MovingStep possibleStep, AstarDecoratorValues parent)
            {
                PossibleStep = possibleStep;
                Parent = parent;
            }
        }
    }
}
