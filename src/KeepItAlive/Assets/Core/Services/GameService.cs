﻿using Assets.Core.DI;
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
                    ContinueRoad(SelectedToon, action);
                    break;
                case GameActionTypes.MoveThere:
                    ScheduleMovementForToon(SelectedToon, action);
                    break;
                case GameActionTypes.MineRock:
                    ScheduleMovementForNeerRockPosition(SelectedToon, action);
                    break;
                case GameActionTypes.PullKing:
                    ScheduleMovementToPullKing(SelectedToon, action);
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

        private void ScheduleMovementToPullKing(ToonScript toon, GameAction action)
        {
            var kingPos = MapService.GetKingPosition();
            var kingX = (int)kingPos.x;
            var kingZ = (int)kingPos.z;

            bool bestFound = false;
            int bestX = kingX;
            int bestZ = kingZ;
            for (int xoff = -1; xoff <= 1; xoff++)
            {
                if (xoff == 0) continue;
                if (!MapService.IsBlockedPosition(kingX + xoff, kingZ))
                {
                    bestX = kingX + xoff;
                    bestZ = kingZ;
                    bestFound = true;
                }
            }

            for (int zoff = -1; zoff <= 1; zoff++)
            {
                if (zoff == 0) continue;
                if (!MapService.IsBlockedPosition(kingX, kingZ + zoff))
                {
                    bestX = kingX;
                    bestZ = kingZ + zoff;
                    bestFound = true;
                }
            }

            if (!bestFound) return;

            var pathFinding = new AstarPathFinding();
            int startX = (int)toon.transform.position.x;
            int startZ = (int)toon.transform.position.z;

            IEnumerable<MovingStep> movingSteps = pathFinding.FromTo(startX, startZ, bestX, bestZ);
            toon.ScheduleMovingSteps(movingSteps);
            toon.SetLifeGoal(ToonLifeGoals.PullKing, action.X, action.Z);
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
                toon.ScheduleMovingSteps(shortestPath);
                toon.SetLifeGoal(ToonLifeGoals.MineRock, action.X, action.Z);
            }
            else
            {
                toon.SetLifeGoal(ToonLifeGoals.Wait, startX, startZ);
            }
        }

        private void ContinueRoad(ToonScript toon, GameAction action)
        {
            var pathFinding = new AstarPathFinding();
            int startX = (int)toon.transform.position.x;
            int startZ = (int)toon.transform.position.z;

            var path = pathFinding.FromTo(startX, startZ, action.X, action.Z);

            toon.ScheduleMovingSteps(path);
            toon.SetLifeGoal(ToonLifeGoals.ContinueRoad, action.X, action.Z);
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
