using Assets.Core.DI;
using Assets.Core.Entities;
using Assets.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Core.Services
{
    public class ActionService : IActionService
    {
        public IMapService MapService => DependencyInjection.Get<IMapService>();

        public IEnumerable<GameAction> GetActionsForCube(CubeTypes type, int x, int z)
        {
            switch (type)
            {
                case CubeTypes.Empty:
                    break;
                case CubeTypes.Grass:
                    return GrassActions(x, z);
                case CubeTypes.Ground:
                    break;
                case CubeTypes.Rock:
                    break;
                case CubeTypes.Iron:
                    break;
                case CubeTypes.Wheat:
                    break;
                case CubeTypes.Tree:
                    break;
                case CubeTypes.Water:
                    break;
                default:
                    break;
            }
            return Enumerable.Empty<GameAction>();
        }

        private IEnumerable<GameAction> GrassActions(int x, int z)
        {
            List<GameAction> actions = new List<GameAction>();

            if (HasAdjacentRoadCube(x, z))
            {
                actions.Add(new GameAction()
                {
                    Text = "Continue road",
                    ActionType = GameActionTypes.ContinueRoad,
                    X = x,
                    Z = z,
                });
            }

            return actions;
        }

        private bool HasAdjacentRoadCube(int x, int z)
        {
            if (MapService.GetCubeType(x - 1, z) == CubeTypes.Road) return true;
            if (MapService.GetCubeType(x + 1, z) == CubeTypes.Road) return true;
            if (MapService.GetCubeType(x, z - 1) == CubeTypes.Road) return true;
            if (MapService.GetCubeType(x, z + 1) == CubeTypes.Road) return true;
            return false;
        }
    }
}
