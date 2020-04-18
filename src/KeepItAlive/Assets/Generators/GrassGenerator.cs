using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject grassPrefab;

    void Start()
    {
        for (int x = 0; x < 50; x++)
        {
            for (int z = 0; z < 25; z++)
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
