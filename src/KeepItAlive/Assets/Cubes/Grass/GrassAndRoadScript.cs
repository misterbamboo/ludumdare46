using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassAndRoadScript : MonoBehaviour
{
    [SerializeField]
    private Material grassMaterial;

    [SerializeField]
    private Material roadMaterial;

    private MeshRenderer renderer;

    private bool isRoad = false;

    private object _lock = new object();

    void Start()
    {
        lock (_lock)
        {
            renderer = GetComponent<MeshRenderer>();
            renderer.material = isRoad ? roadMaterial : grassMaterial;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ConvertToRoad()
    {
        lock (_lock)
        {
            isRoad = true;
            if (renderer != null)
            {
                renderer.material = roadMaterial;
            }
        }
    }

    public void ConvertGrass()
    {
        lock (_lock)
        {
            isRoad = false;
            if (renderer != null)
            {
                renderer.material = grassMaterial;
            }
        }
    }
}
