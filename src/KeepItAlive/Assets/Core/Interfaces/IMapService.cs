using Assets.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Core.Interfaces
{
    public interface IMapService : IService
    {
        CubeTypes GetCubeType(int x, int z);
    }
}
