using Assets.Core.DI;
using Assets.Core.Entities;
using Assets.Core.Interfaces;
using Assets.Core.PathFinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Core.Services
{
    public class GameService : MonoBehaviour, IGameService
    {
        private IMapService MapService { get { return DependencyInjection.Instance.GetService<IMapService>(); } }

        private IActionService ActionService { get { return DependencyInjection.Instance.GetService<IActionService>(); } }

        private IHudService HudService { get { return DependencyInjection.Instance.GetService<IHudService>(); } }

        private ToonScript SelectedToon { get; set; }

        public int RockCount { get; set; } = 10;

        public int WheatCount { get; set; } = 10;

        public int TreeCount { get; set; } = 10;

        public void SelectPosition(int x, int z)
        {
            if (MapService.HasToonAt(x, z))
            {
                SelectedToon = MapService.GetToonAt(x, z);
            }
            else if (SelectedToon != null)
            {
                CubeTypes type = MapService.GetCubeType(x, z);
                var actions = ActionService.GetActionsForCube(type, x, z);

                if (actions.Where(a => a.ActionType != GameActionTypes.Cancel).Any())
                {
                    HudService.UpdateMenuActions(actions);
                    HudService.OpenMenu();
                }
            }
        }

        public void ExecuteGameAction(GameAction action)
        {
            Debug.Log(action.Text);
            switch (action.ActionType)
            {
                case GameActionTypes.ContinueRoad:
                    ContinueRoad(action);
                    break;
                case GameActionTypes.MoveThere:
                    ScheduleMovementForToon(SelectedToon, action);
                    break;
                case GameActionTypes.MineRock:
                    ScheduleMovementForNeerRockPosition(SelectedToon, action);
                    break;
                default:
                    break;
            }
            HudService.CloseMenu();
        }

        private void ScheduleMovementForToon(ToonScript toon, GameAction action)
        {
            var pathFinding = new AstarPathFinding();
            IEnumerable<MovingStep> movingSteps = pathFinding.FromTo((int)toon.transform.position.x, (int)toon.transform.position.z, action.X, action.Z);
            toon.ScheduleMovingSteps(movingSteps);
        }

        private void ScheduleMovementForNeerRockPosition(ToonScript toon, GameAction action)
        {
            var pathFinding = new AstarPathFinding();
            int startX = (int)toon.transform.position.x;
            int startZ = (int)toon.transform.position.z;

            List<IEnumerable<MovingStep>> allPaths = new List<IEnumerable<MovingStep>>();
            for (int xoff = -1; xoff <= 1; xoff++)
            {
                if (xoff == 0) continue;
                if (MapService.IsBlockedPosition(action.X + xoff, action.Z))
                {
                    continue;
                }

                IEnumerable<MovingStep> movingSteps = pathFinding.FromTo(startX, startZ, action.X + xoff, action.Z);
                if (movingSteps.Any())
                {
                    allPaths.Add(movingSteps);
                }
            }

            for (int zoff = -1; zoff <= 1; zoff++)
            {
                if (zoff == 0) continue;
                if (MapService.IsBlockedPosition(action.X, action.Z + zoff))
                {
                    continue;
                }

                IEnumerable<MovingStep> movingSteps = pathFinding.FromTo(startX, startZ, action.X, action.Z + zoff);
                if (movingSteps.Any())
                {
                    allPaths.Add(movingSteps);
                }
            }

            if (allPaths.Any())
            {
                var shortestPath = allPaths.OrderBy(p => p.Count()).First();
                toon.SetLiveGoal(ToonLiveGoals.MineRock);
                toon.ScheduleMovingSteps(shortestPath);
            }
            else
            {
                toon.SetLiveGoal(ToonLiveGoals.Wait);
            }
        }

        private void ContinueRoad(GameAction action)
        {
            MapService.ConvertGrassToRoad(action.X, action.Z);
        }

        public bool IsToonSelected(ToonScript toon)
        {
            return SelectedToon == toon;
        }

        public bool HasSelectedToon()
        {
            return SelectedToon != null;
        }
    }
}
