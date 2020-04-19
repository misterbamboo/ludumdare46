using Assets.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Core.Interfaces
{
    public interface IMapService : IService
    {
        CubeTypes GetCubeType(int x, int z);
        void ConvertGrassToRoad(int x, int z);
        bool HasToonAt(int x, int z);
        ToonScript GetToonAt(int x, int z);
        bool IsBlockedPosition(int startX, int startZ);
        int GetTotalBocks();
    }
}
