using Assets.Core.Interfaces;
using Assets.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Core.DI
{
    public class DependencyInjection : MonoBehaviour, IDependencyInjection
    {
        public static IDependencyInjection Instance { get; private set; }

        private IGameService GameService { get { return gameService; } }

        private IHudService HudService { get { return hudService; } }

        private IMapService MapService { get { return mapService; } }

        private IMouseService MouseService { get { return mouseService; } }

        private IActionService ActionService { get; set; } = new ActionService();
        
        [SerializeField]
        private GameService gameService;

        [SerializeField]
        private HudService hudService;

        [SerializeField]
        private MapService mapService;

        [SerializeField]
        private MouseService mouseService;

        void Start()
        {
            Instance = this;
        }

        void Update()
        {

        }

        public static T Get<T>() where T : IService
        {
            return Instance.GetService<T>();
        }

        public T GetService<T>() where T : IService
        {
            var wantedService = typeof(T);
            if (wantedService == typeof(IHudService))
            {
                return (T)HudService;
            }
            else if (wantedService == typeof(IMapService))
            {
                return (T)MapService;
            }
            else if (wantedService == typeof(IMouseService))
            {
                return (T)MouseService;
            }
            else if (wantedService == typeof(IGameService))
            {
                return (T)GameService;
            }
            else if (wantedService == typeof(IActionService))
            {
                return (T)ActionService;
            }

            throw new NotImplementedException("Not implemented service");
        }
    }
}
