using Assets.Core.Entities;
using Assets.Core.Generators.Algorithms;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject grassPrefab;

    [SerializeField]
    private GameObject rockPrefab;

    [SerializeField]
    private GameObject groundPrefab;

    [SerializeField]
    private GameObject wheatPrefab;

    [SerializeField]
    private GameObject treePrefab;

    [SerializeField]
    private GameObject waterPrefab;

    [SerializeField]
    private int width = 32;

    [SerializeField]
    private int depth = 256;

    [SerializeField]
    private int rockSpotPercent = 20;

    [SerializeField]
    private int treeSpotPercent = 20;

    [SerializeField]
    private int grassSpotPercent = 15;

    [SerializeField]
    private int waterSpotPercent = 25;

    [SerializeField]
    private int wheatSpotPercent = 20;

    [SerializeField]
    private float treeHeight = 0.38f;

    private Map map;

    void Start()
    {
        try
        {
            map = new Map(width, depth);

            GenerateSpots(CubeTypes.Wheat, CubeTypes.Grass, 2, 4, wheatSpotPercent);
            GenerateSpots(CubeTypes.Water, CubeTypes.Water, 8, 16, waterSpotPercent);
            GenerateSpots(CubeTypes.Tree, CubeTypes.Grass, 8, 16, treeSpotPercent);
            GenerateSpots(CubeTypes.Rock, CubeTypes.Ground, 8, 16, rockSpotPercent);
            GenerateSpots(CubeTypes.Grass, CubeTypes.Empty, 5, 12, grassSpotPercent);
            GeneratePath();
            FillMapEmptySpots(CubeTypes.Tree);

            CreateGameObjects();
        }
        catch (System.Exception ex)
        {

        }
    }

    void Update()
    {

    }

    private void GeneratePath()
    {
        var grassPathAlgorithm = new GrassPathAlgorithm(map);
        grassPathAlgorithm.GeneratePath();
    }

    private void GenerateSpots(CubeTypes cubeType, CubeTypes paddingType, int minRadius, int maxRadius, int spotPercent)
    {
        var averageRadius = (minRadius + maxRadius) / 2;
        float approxNumberOfSpots = EvaluateApproxNumberOfSpots(averageRadius, spotPercent);

        var spotAlgorithm = new SpotAlgorithm(map, cubeType, paddingType);
        spotAlgorithm.SpotMinRadius = minRadius;
        spotAlgorithm.SpotMaxRadius = maxRadius;
        for (int i = 0; i < approxNumberOfSpots; i++)
        {
            spotAlgorithm.GenerateSpot();
        }
    }

    private int EvaluateApproxNumberOfSpots(int radius, int spotPercent)
    {
        var circleSurface = Mathf.Pow(radius, 2) * Mathf.PI;
        var totalBlocs = width * depth;
        var totalAllowedBlocs = totalBlocs * spotPercent / 100;
        var approxNumberOfSpots = totalAllowedBlocs / circleSurface;
        return (int)approxNumberOfSpots;
    }

    private void FillMapEmptySpots(CubeTypes cubeType)
    {
        map.FillEmptySpacesWith(cubeType);
    }

    private void CreateGameObjects()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                var type = map.GetCubeType(x, z);
                CreateGameObjectFromCubeType(type, x, z);
            }
        }
    }

    private void CreateGameObjectFromCubeType(CubeTypes cubeType, int x, int z)
    {
        GameObject fieldPrefab = null;
        GameObject overFieldPrefab = null;
        float overFieldHeight = 1;
        float zTilt = 0;
        float xTilt = 0;
        float yRotation = 0;
        float scale = 1;
        switch (cubeType)
        {
            case CubeTypes.Empty:
                break;
            case CubeTypes.Grass:
                fieldPrefab = grassPrefab;
                break;
            case CubeTypes.Ground:
                fieldPrefab = groundPrefab;
                break;
            case CubeTypes.Rock:
                fieldPrefab = rockPrefab;
                break;
            case CubeTypes.Iron:
                break;
            case CubeTypes.Wheat:
                fieldPrefab = grassPrefab;
                overFieldPrefab = wheatPrefab;
                break;
            case CubeTypes.Tree:
                fieldPrefab = groundPrefab;
                overFieldPrefab = treePrefab;
                overFieldHeight = treeHeight;

                zTilt = UnityEngine.Random.Range(-3f, 3f);
                xTilt = UnityEngine.Random.Range(-3f, 3f);
                yRotation = UnityEngine.Random.Range(0, 360);

                scale = UnityEngine.Random.Range(0.9f, 1.2f);
                overFieldHeight = overFieldHeight * scale;
                break;
            case CubeTypes.Water:
                fieldPrefab = waterPrefab;
                break;
            default:
                break;
        }

        if (fieldPrefab != null)
        {
            var gameObject = Instantiate(fieldPrefab);
            gameObject.transform.position = new Vector3(x, 0, z);
        }

        if (overFieldPrefab != null)
        {
            var gameObject = Instantiate(overFieldPrefab);
            gameObject.transform.position = new Vector3(x, overFieldHeight, z);
            gameObject.transform.rotation = Quaternion.Euler(xTilt, yRotation, zTilt);
            gameObject.transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
