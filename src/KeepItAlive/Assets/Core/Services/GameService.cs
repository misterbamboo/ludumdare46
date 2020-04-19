using Assets.Core.DI;
using Assets.Core.Entities;
using Assets.Core.Interfaces;
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

        public void SelectPosition(int x, int z)
        {
            SelectedToon = null;
            if (MapService.HasToonAt(x, z))
            {
                SelectedToon = MapService.GetToonAt(x, z);
            }
            else if(SelectedToon != null)
            {
                CubeTypes type = MapService.GetCubeType(x, z);
                var actions = ActionService.GetActionsForCube(type, x, z);

                if (actions.Any())
                {
                    HudService.UpdateActions(actions);
                    HudService.OpenHud();
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
                default:
                    break;
            }
            HudService.CloseHud();
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
