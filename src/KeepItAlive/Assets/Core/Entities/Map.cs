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
            RessourceQuantity = new int[width][];
            for (int i = 0; i < width; i++)
            {
                Grid[i] = new CubeTypes[depth];
                GameObjectsGrid[i] = new GameObject[depth];
                RessourceQuantity[i] = new int[depth];
            }

            Toons = new List<ToonScript>();
        }

        private CubeTypes[][] Grid { get; set; }

        private int[][] RessourceQuantity { get; set; }

        private GameObject[][] GameObjectsGrid { get; set; }

        private List<ToonScript> Toons { get; set; }

        public void FillEmptySpacesWith(CubeTypes cubeType, int minQty, int maxQty)
        {
            for (int x = 0; x < Grid.Length; x++)
            {
                for (int z = 0; z < Grid[x].Length; z++)
                {
                    if (Grid[x][z] == CubeTypes.Empty)
                    {
                        Grid[x][z] = cubeType;
                        RessourceQuantity[x][z] = UnityEngine.Random.Range(minQty, maxQty);
                    }
                }
            }
        }

        public ToonScript GetToonAt(int x, int z)
        {
            return Toons.Where(t => x == (int)t.transform.position.x && z == (int)t.transform.position.z).FirstOrDefault();
        }

        public void PlaceCube(CubeTypes cubeType, int x, int z, int minQty, int maxQty)
        {
            x = Mathf.Clamp(x, 0, Width - 1);
            z = Mathf.Clamp(z, 0, Depth - 1);
            Grid[x][z] = cubeType;
            RessourceQuantity[x][z] = UnityEngine.Random.Range(minQty, maxQty);
        }

        public int GetRessourceCount(int x, int z)
        {
            return RessourceQuantity[x][z];
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

            var cupeType = GetCubeType(x, z);
            if (cupeType == CubeTypes.Road)
            {
                var pos = gameObject.transform.localPosition;
                pos.y = -0.1f;
                gameObject.transform.localPosition = pos;
            }
            else if (cupeType == CubeTypes.Wheat || cupeType == CubeTypes.Tree || cupeType == CubeTypes.Ground || cupeType == CubeTypes.Grass)
            {
                // do nothing for them
            }
            else
            {
                float quantity = RessourceQuantity[x][z];
                var scale = gameObject.transform.localScale;
                scale.y = quantity <= 10 ? 1 : 1 + quantity / 100f;
                gameObject.transform.localScale = scale;

                var pos = gameObject.transform.localPosition;
                pos.y = (scale.y - 1) / 2;
                gameObject.transform.localPosition = pos;
            }
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
