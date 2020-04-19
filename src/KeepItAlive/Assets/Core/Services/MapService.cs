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
    public class MapService : MonoBehaviour, IMapService
    {
        [SerializeField]
        private MapGenerator mapGenerator;

        [SerializeField]
        private GameObject kingObject;

        private Map Map { get; set; }

        void Start()
        {
            Map = mapGenerator.GenerateMap(kingObject);
        }

        public CubeTypes GetCubeType(int x, int z)
        {
            return Map.GetCubeType(x, z);
        }

        public void ConvertGrassToRoad(int x, int z)
        {
            if (Map.IsCubeType(x, z, CubeTypes.Grass))
            {
                var grassCube = Map.GetGameObject(x, z);
                var script = grassCube.GetComponent<GrassAndRoadScript>();
                script.ConvertToRoad();
                Map.PlaceCube(CubeTypes.Road, x, z);
            }
        }

        public bool HasToonAt(int x, int z)
        {
            var toon = Map.GetToonAt(x, z);
            return toon != null;
        }

        public ToonScript GetToonAt(int x, int z)
        {
            return Map.GetToonAt(x, z);
        }
    }
}