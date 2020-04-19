using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Core.Entities
{
    public class Map
    {
        public int Width { get; private set; }
        public int Depth { get; private set; }

        public Map(int width, int depth)
        {
            this.Width = width;
            this.Depth = depth;
            Grid = new CubeTypes[width][];
            GameObjectsGrid = new GameObject[width][];
            for (int i = 0; i < width; i++)
            {
                Grid[i] = new CubeTypes[depth];
                GameObjectsGrid[i] = new GameObject[depth];
            }

            Toons = new List<ToonScript>();
        }

        private CubeTypes[][] Grid { get; set; }

        private GameObject[][] GameObjectsGrid { get; set; }

        private List<ToonScript> Toons { get; set; }

        public void FillEmptySpacesWith(CubeTypes cubeType)
        {
            for (int x = 0; x < Grid.Length; x++)
            {
                for (int z = 0; z < Grid[x].Length; z++)
                {
                    if (Grid[x][z] == CubeTypes.Empty)
                    {
                        Grid[x][z] = cubeType;
                    }
                }
            }
        }

        public ToonScript GetToonAt(int x, int z)
        {
            return Toons.Where(t => x == (int)t.transform.position.x && z == (int)t.transform.position.z).FirstOrDefault();
        }

        public void PlaceCube(CubeTypes cubeType, int x, int z)
        {
            x = Mathf.Clamp(x, 0, Width - 1);
            z = Mathf.Clamp(z, 0, Depth - 1);
            Grid[x][z] = cubeType;
        }

        public CubeTypes GetCubeType(int x, int z)
        {
            if (x < 0 || x >= Width) return CubeTypes.Empty;
            if (z < 0 || z >= Depth) return CubeTypes.Empty;

            return Grid[x][z];
        }

        public bool IsCubeType(int x, int z, CubeTypes cubeType)
        {
            if (x < 0 || x >= Width) return false;
            if (z < 0 || z >= Depth) return false;

            return Grid[x][z] == cubeType;
        }

        public void PlaceGameObject(GameObject gameObject, int x, int z)
        {
            x = Mathf.Clamp(x, 0, Width - 1);
            z = Mathf.Clamp(z, 0, Depth - 1);

            GameObjectsGrid[x][z] = gameObject;
        }

        public GameObject GetGameObject(int x, int z)
        {
            if (x < 0 || x >= Width) return null;
            if (z < 0 || z >= Depth) return null;

            return GameObjectsGrid[x][z];
        }

        public void AddToon(ToonScript villager)
        {
            if (villager == null) return;
            if (Toons.Contains(villager)) return;
            Toons.Add(villager);
        }
    }
}
