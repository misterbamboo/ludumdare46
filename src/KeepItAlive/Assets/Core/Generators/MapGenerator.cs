using Assets.Core.Entities;
using Assets.Core.Generators.Algorithms;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject grassPrefab;

    [SerializeField]
    private int width;

    [SerializeField]
    private int depth;

    private Map map;

    void Start()
    {
        map = new Map(width, depth);

        var grassPathAlgorithm = new GrassPathAlgorithm(map);
        grassPathAlgorithm.GeneratePath();

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                var type = map.Grid[x][z];
                if (type == CubeTypes.Grass)
                {
                    var grassCube = Instantiate(grassPrefab);
                    grassCube.transform.position = new Vector3(x, 0, z);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
