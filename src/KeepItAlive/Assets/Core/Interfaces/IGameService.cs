using Assets.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Core.Interfaces
{
    public interface IGameService : IService
    {
        void SelectPosition(int x, int z);

        void ExecuteGameAction(GameAction action);
        bool IsToonSelected(ToonScript toonScript);

        bool HasSelectedToon();
    }
}
