using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using TMPro;

public class SphereRandomGenerator : MonoBehaviour
{
    public float lineWidth = 0.7f;
    public GameObject spherePrefab;
    public Material lineMaterial;
    public Color color1 = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    public Color color2 = Color.gray;

    public GameObject displayObj;
    public TMP_Text nameTextDisplay;
    public TMP_Text functionTextDisplay;
    public TMP_Text coordsTextDisplay;
    public TMP_Text neighborsTextDisplay;
    public RectTransform buttonContainer; // Assign your UI panel for buttons in the Unity Editor
    public Button buttonPrefab; // Assign your UI button prefab in the Unity Editor

    private List<GameObject> spheres = new List<GameObject>();
    private List<Node> nodes = new List<Node>();
    private List<LineRenderer> lines = new List<LineRenderer>();
    private GameObject structure;
    private GameObject selectedSphere;

    private List<Vector3> lineInitialPositions = new List<Vector3>();

    // Node class to hold data for each node
    public class Node
    {
        public string Name;
        public string ParentName;
        public string Function;
        public List<string> ConnectedTo;
        public int XCoord;
        public int YCoord;
        public int ZCoord;
    }

    void Start()
    {
        structure = new GameObject("Structure");
        structure.AddComponent<RotateAndZoom>();
        GenerateNodesFromData();
        GenerateSpheres();
        CreateConnectionLines();
        SaveLineInitialPositions();
    }

    void GenerateNodesFromData()
    {
        string fileName = "Main";

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
                        Name = values[1],
                        ParentName = values[0],
                        Function = values[2].Trim('"'),
                        XCoord = int.Parse(values[4]),
                        YCoord = int.Parse(values[5]),
                        ZCoord = int.Parse(values[6]),
                        ConnectedTo = new List<string>(values[3].Split(';')),
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

            nodeComponent.Name = node.Name;
            nodeComponent.ParentName = node.ParentName;
            nodeComponent.Function = node.Function;
            nodeComponent.XCoord = node.XCoord;
            nodeComponent.YCoord = node.YCoord;
            nodeComponent.ZCoord = node.ZCoord;
            nodeComponent.ConnectedTo = new List<string>(node.ConnectedTo);


            // Store the Node data in the NodeComponent
            nodeComponent.NodeData = new Node
            {
                Name = node.Name,
                ParentName = node.ParentName,
                Function = node.Function,
                ConnectedTo = new List<string>(node.ConnectedTo),
                XCoord = node.XCoord,
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
            // create lines for connected to
            NodeComponent nodeComponent = sphere.GetComponent<NodeComponent>();
            foreach (string connectedNodeName in nodeComponent.ConnectedTo)
            {
                GameObject connectedSphere = FindSphere(connectedNodeName);
                if (connectedSphere != null)
                {   
                    CreateLine(sphere, connectedSphere, color1);
                }
            }

        // create line to parent node, has different color
            if(nodeComponent.ParentName != " ")
            {
                GameObject parentSphere = FindSphere(nodeComponent.ParentName);
                if (parentSphere != null)
                {   
                    Debug.Log("Hello, Unity Console!");
                    CreateLine(sphere, parentSphere, color2);
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


    void CreateLine(GameObject startSphere, GameObject endSphere, Color color)
    {
        LineRenderer lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();

        lineRenderer.material = lineMaterial;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startSphere.transform.position);
        lineRenderer.SetPosition(1, endSphere.transform.position);
        lineRenderer.transform.parent = structure.transform;  // Parent the line to the structure
        lineRenderer.material.color = color;

        lines.Add(lineRenderer);
    }

    void Update()
    {
        UpdateLinePositions();
        HandleSphereSelection();
    }

    void HandleSphereSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.collider.gameObject;

                if (spheres.Contains(hitObject))
                {
                    selectedSphere = hitObject;
                    UpdateTextDisplay(selectedSphere.GetComponent<NodeComponent>().NodeData);
                }
            }
        }
    }

    void UpdateTextDisplay(Node node)
    {
        displayObj.SetActive(true);
        if (nameTextDisplay != null && functionTextDisplay != null && coordsTextDisplay != null && neighborsTextDisplay != null)
        {
            nameTextDisplay.text = $"{node.Name}";
            functionTextDisplay.text = $"Function: {node.Function}";
            coordsTextDisplay.text = $"Coords: ({node.XCoord}, {node.YCoord}, {node.ZCoord})";

            // Display neighboring nodes
            neighborsTextDisplay.text = "Neighbors: " + string.Join(", ", node.ConnectedTo.ToArray());

            // Create buttons for neighboring nodes
            CreateNeighborButtons(node);
        }
    }

    void CreateNeighborButtons(Node node)
    {
        // Clear existing buttons
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }

        // Create buttons for each neighboring node
        foreach (string neighborName in node.ConnectedTo)
        {
            Button neighborButton = Instantiate(buttonPrefab, buttonContainer);
            neighborButton.GetComponentInChildren<TMP_Text>().text = neighborName;

            // Add a click event to the button
            neighborButton.onClick.AddListener(() => GoToNeighborNode(neighborName));
        }
    }

    void GoToNeighborNode(string neighborName)
    {
        // Find and select the sphere corresponding to the clicked neighbor node
        GameObject neighborSphere = FindSphere(neighborName);
        if (neighborSphere != null)
        {
            selectedSphere = neighborSphere;
            UpdateTextDisplay(selectedSphere.GetComponent<NodeComponent>().NodeData);
        }
    }
}
