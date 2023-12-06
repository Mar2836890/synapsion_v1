using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InstantiateNodes : MonoBehaviour
{
    public int gridSizeX = 5;
    public int gridSizeY = 5;
    public int gridSizeZ = 5;
    public float spacing = 2.0f;
    public float lineWidth = 0.1f;
    public GameObject spherePrefab;
    public Material lineMaterial;

    private List<GameObject> spheres = new List<GameObject>();
    private List<LineRenderer> lines = new List<LineRenderer>();

    void Start()
    {
        GenerateGrid();
        CreateConnectionLines();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                for (int z = 0; z < gridSizeZ; z++)
                {
                    Vector3 spawnPosition = new Vector3(x * spacing, y * spacing, z * spacing);
                    GameObject sphere = Instantiate(spherePrefab, spawnPosition, Quaternion.identity, transform);
                    spheres.Add(sphere);
                }
            }
        }
    }

    void CreateConnectionLines()
    {
        foreach (GameObject sphere in spheres)
        {
            List<GameObject> closestSpheres = FindClosestSpheres(sphere, 3);
            foreach (GameObject closestSphere in closestSpheres)
            {
                CreateLine(sphere, closestSphere);
            }
        }
    }

    List<GameObject> FindClosestSpheres(GameObject targetSphere, int numSpheres)
    {
        List<GameObject> closestSpheres = new List<GameObject>();

        List<GameObject> sortedSpheres = new List<GameObject>(spheres);
        sortedSpheres.Remove(targetSphere);
        sortedSpheres.Sort((a, b) => Vector3.Distance(targetSphere.transform.position, a.transform.position).CompareTo(Vector3.Distance(targetSphere.transform.position, b.transform.position)));

        for (int i = 0; i < Mathf.Min(numSpheres, sortedSpheres.Count); i++)
        {
            closestSpheres.Add(sortedSpheres[i]);
        }

        return closestSpheres;
    }

    void CreateLine(GameObject startSphere, GameObject endSphere)
    {
        LineRenderer lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startSphere.transform.position);
        lineRenderer.SetPosition(1, endSphere.transform.position);
        lines.Add(lineRenderer);
    }
}