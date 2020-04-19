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

        public void SelectPosition(int x, int z)
        {
            CubeTypes type = MapService.GetCubeType(x, z);
            var actions = ActionService.GetActionsForCube(type);

            HudService.UpdateActions(actions);
            HudService.OpenHud();
        }
    }
}
