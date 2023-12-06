using UnityEngine;
using System.Collections.Generic;

public class SphereRandomGenerator : MonoBehaviour
{
    public int numSpheres = 50;
    public float randomRange = 10.0f;
    public float lineWidth = 0.1f;
    public GameObject spherePrefab;
    public Material lineMaterial;

    private List<GameObject> spheres = new List<GameObject>();
    private List<LineRenderer> lines = new List<LineRenderer>();
    private GameObject structure;

    private List<Vector3> lineInitialPositions = new List<Vector3>();

    void Start()
    {
        structure = new GameObject("Structure");
        structure.AddComponent<RotateAndZoom>();  // Add your custom script here
        GenerateRandomSpheres();
        CreateConnectionLines();
        SaveLineInitialPositions();
    }

    void GenerateRandomSpheres()
    {
        for (int i = 0; i < numSpheres; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(-randomRange, randomRange),
                                                 Random.Range(-randomRange, randomRange),
                                                 Random.Range(-randomRange, randomRange));

            GameObject sphere = Instantiate(spherePrefab, randomPosition, Quaternion.identity, structure.transform);
            spheres.Add(sphere);
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

    void SaveLineInitialPositions()
    {
        foreach (LineRenderer lineRenderer in lines)
        {
            lineInitialPositions.Add(lineRenderer.GetPosition(0));
            lineInitialPositions.Add(lineRenderer.GetPosition(1));
        }
    }

    void UpdateLinePositions()
    {
        int index = 0;
        foreach (LineRenderer lineRenderer in lines)
        {
            Vector3 initialStartPos = lineInitialPositions[index++];
            Vector3 initialEndPos = lineInitialPositions[index++];
            Vector3 newStartPos = structure.transform.TransformPoint(initialStartPos);
            Vector3 newEndPos = structure.transform.TransformPoint(initialEndPos);

            lineRenderer.SetPosition(0, newStartPos);
            lineRenderer.SetPosition(1, newEndPos);
        }
    }

    List<GameObject> FindClosestSpheres(GameObject targetSphere, int numSpheres)
    {
        List<GameObject> closestSpheres = new List<GameObject>();

        List<GameObject> sortedSpheres = new List<GameObject>(spheres);
        sortedSpheres.Remove(targetSphere);
        sortedSpheres.Sort((a, b) => Vector3.Distance(targetSphere.transform.position, a.transform.position)
                                     .CompareTo(Vector3.Distance(targetSphere.transform.position, b.transform.position)));

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
        lineRenderer.transform.parent = structure.transform;  // Parent the line to the structure
        lines.Add(lineRenderer);
    }

    void Update()
    {
        UpdateLinePositions();
    }
}