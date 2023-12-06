using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class SphereRandomGenerator : MonoBehaviour
{

    public float lineWidth = 0.7f;
    public GameObject spherePrefab;
    public Material lineMaterial;

    private List<GameObject> spheres = new List<GameObject>();
    private List<Node> nodes = new List<Node>();
    private List<LineRenderer> lines = new List<LineRenderer>();
    private GameObject structure;

    private List<Vector3> lineInitialPositions = new List<Vector3>();


    // Node class to hold data for each node
    public class Node
    {
        public string Name;
        public string Function;
        public List<string> ConnectedTo;
        public int XCoord;
        public int YCoord;
        public int ZCoord;
    }


    void Start()
    {
        structure = new GameObject("Structure");
        structure.AddComponent<RotateAndZoom>();  // Add your custom script here
        GenerateNodesFromData();
        GenerateSpheres();
        CreateConnectionLines();
        SaveLineInitialPositions();
    }



    void GenerateNodesFromData(){
        string fileName = "dataNodesCoords";

        // Load the text file from the Resources folder
        TextAsset textAsset = Resources.Load<TextAsset>(fileName);
        if (textAsset != null)
        {
            using (StringReader reader = new StringReader(textAsset.text))
            {
                reader.ReadLine(); // Skip the header

                while (reader.Peek() != -1)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(',');

                    // Create a new Node instance and populate its data
                    Node node = new Node
                    {
                        Name = values[0],
                        Function = values[1].Trim('"'),
                        XCoord = int.Parse(values[3]),
                        YCoord = int.Parse(values[4]),
                        ZCoord = int.Parse(values[5]),
                        ConnectedTo = new List<string>(values[2].Split(';')),
                    };

                    nodes.Add(node);
                }
            }
        }
        else
        {
            Debug.LogError($"Error loading CSV file: {fileName}");
        }
    }


    void GenerateSpheres()
    {
        foreach (Node node in nodes)
        {
            // Create a sphere for each node
            Vector3 position = new Vector3(node.XCoord, node.YCoord, node.ZCoord);
            GameObject sphere = Instantiate(spherePrefab, position, Quaternion.identity, structure.transform);


            NodeComponent nodeComponent = sphere.GetComponent<NodeComponent>();

            // shows val on console
            nodeComponent.Name = node.Name;
            nodeComponent.Function = node.Function;
            nodeComponent.XCoord = node.XCoord;
            nodeComponent.YCoord = node.YCoord;
            nodeComponent.ZCoord = node.ZCoord;

            nodeComponent.ConnectedTo = new List<string>();
            // Add connected nodes to the ConnectedTo list
            nodeComponent.ConnectedTo.AddRange(node.ConnectedTo);



            // Store the Node data in the NodeComponent
            nodeComponent.NodeData = new Node
            {
                Name = node.Name,
                Function = node.Function,
                ConnectedTo = new List<string>(node.ConnectedTo),
                XCoord= node.XCoord,
                YCoord = node.YCoord,
                ZCoord = node.ZCoord
            };
            
            spheres.Add(sphere);

        }

    }

    void CreateConnectionLines()
    {
        foreach (GameObject sphere in spheres)
        {   
            NodeComponent nodeComponent = sphere.GetComponent<NodeComponent>();
            Debug.Log($"NODE {nodeComponent.Name}");
            foreach (string connectedNodeName in nodeComponent.ConnectedTo)
            {   
                Debug.Log($"COOONECT {connectedNodeName}");
                GameObject connectedSphere = FindSphere(connectedNodeName);
                if (connectedSphere != null)
                {   
                    Debug.Log($"MAKDE LINE");
                    CreateLine(sphere, connectedSphere);
                }
            }
        }
    }

    private GameObject FindSphere(string nodeName)
    {
        foreach (var sphere in spheres)
        {
            NodeComponent nodeComponent = sphere.GetComponent<NodeComponent>();
            if (nodeComponent.Name == nodeName.TrimStart())
            {
                return sphere;
            }
        }
        return null;
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







// using UnityEngine;
// using System.Collections.Generic;
// using System.IO;

// public class SphereRandomGenerator : MonoBehaviour
// {
//     public int numSpheres = 50;
//     public float randomRange = 10.0f;
//     public float lineWidth = 0.1f;
//     public GameObject spherePrefab;
//     public Material lineMaterial;

//     private List<GameObject> spheres = new List<GameObject>();
//     private List<LineRenderer> lines = new List<LineRenderer>();
//     private GameObject structure;

//     private List<Vector3> lineInitialPositions = new List<Vector3>();

//     void Start()
//     {
//         structure = new GameObject("Structure");
//         structure.AddComponent<RotateAndZoom>();  // Add your custom script here
//         GenerateRandomSpheres();
//         CreateConnectionLines();
//         SaveLineInitialPositions();
//     }

//     void GenerateRandomSpheres()
//     {
//         string fileName = "dataNodesCoords";

//         // Load the text file from the Resources folder
//         TextAsset textAsset = Resources.Load<TextAsset>(fileName);
//         if (textAsset != null)
//         {
//             using (StringReader reader = new StringReader(textAsset.text))
//             {   
//                 // skips first line
//                 reader.ReadLine();

//                 // reads rest of the lines
//                 while (reader.Peek() != -1)
//                 {
//                     string line = reader.ReadLine();
//                     string[] values = line.Split(',');
//                     Vector3 randomPosition = new Vector3(int.Parse(values[3]), int.Parse(values[4]), int.Parse(values[5]));

//                     GameObject sphere = Instantiate(spherePrefab, randomPosition, Quaternion.identity, structure.transform);
//                     spheres.Add(sphere);

//                 }
//             }
//         }
//         else
//         {
//             Debug.LogError($"Error loading CSV file: {fileName}");
//         }
        
//     }

//     void CreateConnectionLines()
//     {
//         foreach (GameObject sphere in spheres)
//         {
//             List<GameObject> closestSpheres = FindClosestSpheres(sphere, 3);
//             foreach (GameObject closestSphere in closestSpheres)
//             {
//                 CreateLine(sphere, closestSphere);
//             }
//         }
//     }

//     void SaveLineInitialPositions()
//     {
//         foreach (LineRenderer lineRenderer in lines)
//         {
//             lineInitialPositions.Add(lineRenderer.GetPosition(0));
//             lineInitialPositions.Add(lineRenderer.GetPosition(1));
//         }
//     }

//     void UpdateLinePositions()
//     {
//         int index = 0;
//         foreach (LineRenderer lineRenderer in lines)
//         {
//             Vector3 initialStartPos = lineInitialPositions[index++];
//             Vector3 initialEndPos = lineInitialPositions[index++];
//             Vector3 newStartPos = structure.transform.TransformPoint(initialStartPos);
//             Vector3 newEndPos = structure.transform.TransformPoint(initialEndPos);

//             lineRenderer.SetPosition(0, newStartPos);
//             lineRenderer.SetPosition(1, newEndPos);
//         }
//     }

//     List<GameObject> FindClosestSpheres(GameObject targetSphere, int numSpheres)
//     {
//         List<GameObject> closestSpheres = new List<GameObject>();

//         List<GameObject> sortedSpheres = new List<GameObject>(spheres);
//         sortedSpheres.Remove(targetSphere);
//         sortedSpheres.Sort((a, b) => Vector3.Distance(targetSphere.transform.position, a.transform.position)
//                                      .CompareTo(Vector3.Distance(targetSphere.transform.position, b.transform.position)));

//         for (int i = 0; i < Mathf.Min(numSpheres, sortedSpheres.Count); i++)
//         {
//             closestSpheres.Add(sortedSpheres[i]);
//         }

//         return closestSpheres;
//     }

//     void CreateLine(GameObject startSphere, GameObject endSphere)
//     {
//         LineRenderer lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();
//         lineRenderer.material = lineMaterial;
//         lineRenderer.startWidth = lineWidth;
//         lineRenderer.endWidth = lineWidth;
//         lineRenderer.positionCount = 2;
//         lineRenderer.SetPosition(0, startSphere.transform.position);
//         lineRenderer.SetPosition(1, endSphere.transform.position);
//         lineRenderer.transform.parent = structure.transform;  // Parent the line to the structure
//         lines.Add(lineRenderer);
//     }

//     void Update()
//     {
//         UpdateLinePositions();
//     }
// }







