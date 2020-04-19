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
        public IEnumerable<GameAction> GetActionsForCube(CubeTypes type)
        {
            switch (type)
            {
                case CubeTypes.Empty:
                    break;
                case CubeTypes.Grass:
                    return GrassActions();
                    break;
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

        private IEnumerable<GameAction> GrassActions()
        {
            return new GameAction[] 
            {
                new GameAction()
                {
                    Text = "Install road"
                },
            };
        }
    }
}
