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
                Map.PlaceCube(CubeTypes.Road, x, z, 1, 1);
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

        public bool IsBlockedPosition(int x, int z)
        {
            var kingX = (int)Map.King.transform.position.x;
            var kingZ = (int)Map.King.transform.position.z;

            if (kingX == x && kingZ == z) return true;

            if (HasToonAt(x, z))
            {
                return true;
            }

            var cube = GetCubeType(x, z);
            switch (cube)
            {
                case CubeTypes.Grass:
                case CubeTypes.Ground:
                case CubeTypes.Wheat:
                case CubeTypes.Road:
                    return false;
                default:
                    return true;
            }
        }

        public int GetTotalBocks()
        {
            return Map.Width * Map.Depth;
        }

        public int GetRessourceCount(int x, int z)
        {
            return Map.GetRessourceCount(x, z);
        }

        public void RemoveRessource(int x, int z)
        {
            Map.RemoveRessource(x, z);
        }

        public Vector3 GetMapEndPoint()
        {
            return Map.EndPoint;
        }

        public Vector3 GetKingPosition()
        {
            return new Vector3(Map.King.transform.position.x, Map.King.transform.position.y, Map.King.transform.position.z);
        }

        public void SetKingPosition(Vector3 vector3)
        {
            Map.King.transform.position = vector3;
        }
    }
}