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

    void Start()
    {
        for (int x = width / -2; x < width; x++)
        {
            for (int z = depth / -2; z < depth; z++)
            {
                var grassCube = Instantiate(grassPrefab);
                grassCube.transform.position = new Vector3(x, 0, z);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
