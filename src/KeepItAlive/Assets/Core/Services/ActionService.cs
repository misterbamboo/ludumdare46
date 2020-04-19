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
                    return GroundActions(x, z);
                case CubeTypes.Rock:
                    return GetRockActions(x, z);
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

        private IEnumerable<GameAction> GetRockActions(int x, int z)
        {
            List<GameAction> actions = new List<GameAction>();

            if (HasAdjacentFreeToPass(x, z))
            {
                actions.Add(new GameAction()
                {
                    Text = "Mine rock",
                    ActionType = GameActionTypes.MineRock,
                    X = x,
                    Z = z,
                });
            }

            AddCancelAction(actions);
            return actions;
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

            actions.Add(new GameAction()
            {
                Text = "Move there",
                ActionType = GameActionTypes.MoveThere,
                X = x,
                Z = z,
            });

            AddCancelAction(actions);
            return actions;
        }

        private IEnumerable<GameAction> GroundActions(int x, int z)
        {
            List<GameAction> actions = new List<GameAction>();
            actions.Add(new GameAction()
            {
                Text = "Move there",
                ActionType = GameActionTypes.MoveThere,
                X = x,
                Z = z,
            });

            AddCancelAction(actions);
            return actions;
        }

        private void AddCancelAction(List<GameAction> actions)
        {
            actions.Add(new GameAction()
            {
                Text = "Cancel",
                ActionType = GameActionTypes.Cancel,
            });
        }

        private bool HasAdjacentRoadCube(int x, int z)
        {
            if (MapService.GetCubeType(x - 1, z) == CubeTypes.Road) return true;
            if (MapService.GetCubeType(x + 1, z) == CubeTypes.Road) return true;
            if (MapService.GetCubeType(x, z - 1) == CubeTypes.Road) return true;
            if (MapService.GetCubeType(x, z + 1) == CubeTypes.Road) return true;
            return false;
        }

        private bool HasAdjacentFreeToPass(int x, int z)
        {
            for (int xoff = -1; xoff < 1; xoff++)
            {
                for (int zoff = -1; zoff < 1; zoff++)
                {
                    if (xoff == 0 && zoff == 0) continue;
                    if (!MapService.IsBlockedPosition(x + xoff, z + zoff)) return true;
                }
            }
            return false;
        }
    }
}
