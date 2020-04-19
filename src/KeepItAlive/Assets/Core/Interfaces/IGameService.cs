﻿using Assets.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Core.Interfaces
{
    public interface IGameService : IService
    {
        int RockCount { get; }
        int WheatCount { get; }
        int TreeCount { get; }

        void SelectPosition(int x, int z);

        void ExecuteGameAction(GameAction action);
        bool IsToonSelected(ToonScript toonScript);

        bool HasSelectedToon();
    }
}
