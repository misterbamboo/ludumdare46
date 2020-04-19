using Assets.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Core.Interfaces
{
    public interface IHudService : IService
    {
        void UpdateActions(IEnumerable<GameAction> actions);

        void OpenHud();

        void CloseHud();
    }
}
