using Assets.Core.Entities;
using Assets.Core.Generators.Algorithms;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject villagerPrefab;

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
    private int rockSpotPercent = 40;

    [SerializeField]
    private int treeSpotPercent = 10;

    [SerializeField]
    private int grassSpotPercent = 10;

    [SerializeField]
    private int waterSpotPercent = 30;

    [SerializeField]
    private int wheatSpotPercent = 10;

    [SerializeField]
    private float treeHeight = 0.38f;

    private Map map;

    public Map GenerateMap(GameObject king)
    {
        map = new Map(width, depth);

        GenerateSpots(CubeTypes.Wheat, CubeTypes.Grass, 2, 4, wheatSpotPercent, 10, 10);
        GenerateSpots(CubeTypes.Rock, CubeTypes.Ground, 3, 20, rockSpotPercent, 150, 200);
        GenerateSpots(CubeTypes.Water, CubeTypes.Water, 4, 14, waterSpotPercent, 1, 1);
        GenerateSpots(CubeTypes.Tree, CubeTypes.Grass, 8, 16, treeSpotPercent, 10, 100);
        GenerateSpots(CubeTypes.Grass, CubeTypes.Empty, 5, 12, grassSpotPercent, 1, 1);
        GeneratePath(king);
        FillMapEmptySpots(CubeTypes.Tree, 10, 100);

        CreateGameObjects();
        return map;
    }

    private void GenerateSpots(CubeTypes cubeType, CubeTypes paddingType, int minRadius, int maxRadius, int spotPercent, int minQty, int maxQty)
    {
        var averageRadius = (minRadius + maxRadius) / 2;
        float approxNumberOfSpots = EvaluateApproxNumberOfSpots(averageRadius, spotPercent);

        var spotAlgorithm = new SpotAlgorithm(map, cubeType, paddingType);
        spotAlgorithm.SpotMinRadius = minRadius;
        spotAlgorithm.SpotMaxRadius = maxRadius;
        for (int i = 0; i < approxNumberOfSpots; i++)
        {
            spotAlgorithm.GenerateSpot(minQty, maxQty);
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

    private void GeneratePath(GameObject king)
    {
        var grassPathAlgorithm = new GrassPathAlgorithm(map);
        grassPathAlgorithm.GeneratePath();

        king.transform.position = grassPathAlgorithm.StartingPoint;
        Camera.main.transform.position = grassPathAlgorithm.StartingPoint;

        for (int i = -2; i <= 2; i++)
        {
            if (i == 0) continue;

            var villager = Instantiate(villagerPrefab);
            villager.transform.position = new Vector3(grassPathAlgorithm.StartingPoint.x + i, grassPathAlgorithm.StartingPoint.y, grassPathAlgorithm.StartingPoint.z);
            var villagerToon = villager.GetComponent<ToonScript>();
            map.AddToon(villagerToon);
        }

        map.SetKing(king);
    }

    private void FillMapEmptySpots(CubeTypes cubeType, int minQty, int maxQty)
    {
        map.FillEmptySpacesWith(cubeType, minQty, maxQty);
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
            case CubeTypes.Road:
                fieldPrefab = grassPrefab;
                break;
            default:
                break;
        }

        if (fieldPrefab != null)
        {
            var gameObject = Instantiate(fieldPrefab);
            gameObject.transform.position = new Vector3(x, 0, z);
            ApplyGrassAndRoadScript(cubeType, gameObject);

            map.PlaceGameObject(gameObject, x, z);
        }

        if (overFieldPrefab != null)
        {
            var gameObject = Instantiate(overFieldPrefab);
            gameObject.transform.position = new Vector3(x, overFieldHeight, z);
            gameObject.transform.rotation = Quaternion.Euler(xTilt, yRotation, zTilt);
            gameObject.transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    private void ApplyGrassAndRoadScript(CubeTypes cubeType, GameObject gameObject)
    {
        var grassAndRoadScript = gameObject.GetComponent<GrassAndRoadScript>();
        if (grassAndRoadScript == null) return;

        switch (cubeType)
        {
            case CubeTypes.Grass:
                grassAndRoadScript.ConvertGrass();
                break;
            case CubeTypes.Road:
                grassAndRoadScript.ConvertToRoad();
                break;
            default:
                break;
        }
    }
}
