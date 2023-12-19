using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using System.Globalization;
public class SphereRandomGenerator : MonoBehaviour
{
    public float lineWidth = 0.11f;
    public GameObject spherePrefab;
    public Material lineMaterialWhite;
    public Material lineMaterialBlack;
    public Color highlightColour = Color.white;
    public float highlightSize = 10f;
    // 1 red, 2 blue, 3 yellow, 4 green, 5 purple
    public List<Color> sphereColors = new List<Color> {
        Color.black,
        new Color(1.0f, 0.678f, 0.678f, 1.0f),
        new Color(0.608f, 0.965f, 1.0f, 1.0f),
        new Color(0.992f, 1.0f, 0.714f, 1.0f),
        new Color(0.792f, 1.0f, 0.749f, 1.0f),
        new Color(0.741f, 0.698f, 1.0f, 1.0f)
    };
    public GameObject displayObj;
    public Image imageComponent;
    public TMP_Text nameTextDisplay;
    public TMP_Text functionTextDisplay;
    public TMP_Text coordsTextDisplay;
    public TMP_Text neighborsTextDisplay;
    public RectTransform buttonContainer; // Assign your UI panel for buttons in the Unity Editor
    public Button buttonPrefab; // Assign your UI button prefab in the Unity Editor
    private Dictionary<GameObject, Color> originalSphereColors = new Dictionary<GameObject, Color>();
    private List<GameObject> spheres = new List<GameObject>();
    private List<Node> nodes = new List<Node>();
    public List<string> ListNodeNames = new List<string>();
    private List<LineRenderer> lines = new List<LineRenderer>();
    private GameObject structure;
    public GameObject selectedSphere;
    private Outline selectedOutline; // New variable to store the Outline component of the selected sphere
    private List<Vector3> lineInitialPositions = new List<Vector3>();
    // toggle button
    public Toggle lineToggle;

    // For brain mode
    public GameObject BrainModel;
    public Toggle BrainToggle;
    public GameObject Transparecny;
    // for the search bar
    public TMP_InputField searchInputField;
    public Button searchButton;
    public GameObject WarningText;
    public float displayTime = 2f;
    // Node class to hold data for each node
    public class Node
    {
        public string Name;
        public string ParentName;
        public string Function;
        public List<string> ConnectedTo;
        public float XCoord;
        public float YCoord;
        public float ZCoord;
        public int NumEntry;
    }
    void Start()
    {
        structure = new GameObject("Structure");
        structure.transform.parent = transform;  // Set the parent of the structure GameObject
        GenerateNodesFromData();
        GenerateSpheres();
        CreateConnectionLines();
        SaveLineInitialPositions();
        Button closeButton = displayObj.GetComponentInChildren<Button>();
        if (closeButton != null)
        {
            // Add a click event to the close button to quit the text display
            closeButton.onClick.AddListener(() => QuitTextDisplay());
        }
        else
        {
            Debug.LogError("Close button not found in the display object.");
        }
        //search button
        searchButton.onClick.AddListener(() => SearchNode());
        // toogle button
        lineToggle.onValueChanged.AddListener(OnLineToggleValueChanged);
        BrainToggle.onValueChanged.AddListener(OnBrainToggleChange);
    }
    void GenerateNodesFromData()
    {
        string fileName = "MainV2a";
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
                    string[] values = line.Split(';');
                    // Create a new Node instance and populate its data
                    Node node = new Node
                    {
                        Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(values[1]),
                        ParentName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(values[0]),
                        Function = values[2],
                        XCoord = float.Parse(values[4], CultureInfo.InvariantCulture),
                        YCoord = float.Parse(values[5], CultureInfo.InvariantCulture),
                        ZCoord = float.Parse(values[6], CultureInfo.InvariantCulture),
                        NumEntry = int.Parse(values[7]),
                        ConnectedTo = new List<string>(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(values[3]).Split(',')),
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
            Vector3 position = new Vector3(node.XCoord, node.YCoord, node.ZCoord);
            GameObject sphere = Instantiate(spherePrefab, position, Quaternion.identity, structure.transform);
            NodeComponent nodeComponent = sphere.GetComponent<NodeComponent>();
            nodeComponent.Name = node.Name;
            nodeComponent.ParentName = node.ParentName;
            nodeComponent.Function = node.Function;
            nodeComponent.XCoord = node.XCoord;
            nodeComponent.YCoord = node.YCoord;
            nodeComponent.ZCoord = node.ZCoord;
            nodeComponent.NumEntry = node.NumEntry;
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
                ZCoord = node.ZCoord,
                NumEntry = node.NumEntry
            };
            Renderer renderer = sphere.GetComponent<Renderer>();
            renderer.material.color = sphereColors[node.NumEntry];
            ListNodeNames.Add(node.Name);
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
                    CreateLine(sphere, connectedSphere, lineMaterialWhite);
                }
            }
            // create line to parent node, has different color
            if (nodeComponent.ParentName != "Parent Area")
            {
                GameObject parentSphere = FindSphere(nodeComponent.ParentName);
                if (parentSphere != null)
                {
                    CreateLine(sphere, parentSphere, lineMaterialBlack);
                }
            }
        }
    }
    public GameObject FindSphere(string nodeName)
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
    void CreateLine(GameObject startSphere, GameObject endSphere, Material material)
    {
        LineRenderer lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();
        lineRenderer.material = material;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = 2;
        // Use local positions of the spheres
        lineRenderer.SetPosition(0, startSphere.transform.localPosition);
        lineRenderer.SetPosition(1, endSphere.transform.localPosition);
        lines.Add(lineRenderer);
    }
    void Update()
    {
        UpdateLinePositions();
        HandleSphereSelection();
        // Check for enter key press in the search bar
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SearchNode();
        }
    }

    // brain mode
    void OnBrainToggleChange(bool visable)
    {   
        if (BrainToggle.isOn == false)
        {
            BrainModel.SetActive(true);
            Transparecny.SetActive(true);
        }
        else
        {
            BrainModel.SetActive(false);
            Transparecny.SetActive(false);
        }
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
                SetSelectedSphere(hitObject);
            }
        }
    }

    void SetSelectedSphere(GameObject newSelectedSphere)
    {
        // Reset the color and outline of the previously selected sphere
        if (selectedSphere != null)
        {
            Renderer existingSelectedRenderer = selectedSphere.GetComponent<Renderer>();
            existingSelectedRenderer.material.color = originalSphereColors[selectedSphere];
            // Reset the outline width
            if (selectedOutline != null)
            {
                selectedOutline.OutlineWidth = 0f;
            }
        }

        selectedSphere = newSelectedSphere;
        Renderer selectedRenderer = selectedSphere.GetComponent<Renderer>();  // Corrected line

        // Store the original color of the selected sphere
        if (!originalSphereColors.ContainsKey(selectedSphere))
        {
            originalSphereColors.Add(selectedSphere, selectedRenderer.material.color);
        }

        // Get or add the Outline component
        selectedOutline = selectedSphere.GetComponent<Outline>();
        if (selectedOutline == null)
        {
            selectedOutline = selectedSphere.AddComponent<Outline>();
        }

        // Set the outline width for the selected sphere
        selectedOutline.OutlineWidth = highlightSize;
        selectedOutline.OutlineColor = highlightColour;

        UpdateTextDisplay(selectedSphere.GetComponent<NodeComponent>().NodeData);
        lineToggle.isOn = false;
    }


    public void SelectNodeByName(string nodeName)
    {
        GameObject selectedSphere = FindSphere(nodeName);
        if (selectedSphere != null)
        {
            SetSelectedSphere(selectedSphere);
        }
    }

    void UpdateTextDisplay(Node node)
    {
        displayObj.SetActive(true);
        if (nameTextDisplay != null && functionTextDisplay != null && coordsTextDisplay != null && neighborsTextDisplay != null)
        {
            imageComponent.color = sphereColors[node.NumEntry];
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
            if (ListNodeNames.Contains(neighborName.TrimStart()))
            {
                Button neighborButton = Instantiate(buttonPrefab, buttonContainer);
                neighborButton.GetComponentInChildren<TMP_Text>().text = neighborName;
                neighborButton.GetComponentInChildren<TMP_Text>().color = Color.white;
                // Add a click event to the button
                neighborButton.onClick.AddListener(() => GoToNeighborNode(neighborName));
            }
        }
    }
    void GoToNeighborNode(string neighborName)
    {       
        // Reset the color and outline of the previously selected sphere
        if (selectedSphere != null)
        {
            Renderer selectedRenderer = selectedSphere.GetComponent<Renderer>();
            selectedRenderer.material.color = originalSphereColors[selectedSphere];
            // Reset the outline width
            if (selectedOutline != null)
            {
                selectedOutline.OutlineWidth = 0f;
            }
        }
        // Find and select the sphere corresponding to the clicked neighbor node
        GameObject neighborSphere = FindSphere(neighborName);
        if (neighborSphere != null)
        {
            selectedSphere = neighborSphere;
            Renderer selectedRenderer = selectedSphere.GetComponent<Renderer>();
            // Store the original color of the selected sphere
            if (!originalSphereColors.ContainsKey(selectedSphere))
            {
                originalSphereColors.Add(selectedSphere, selectedRenderer.material.color);
            }
            // Get or add the Outline component
            selectedOutline = selectedSphere.GetComponent<Outline>();
            if (selectedOutline == null)
            {
                selectedOutline = selectedSphere.AddComponent<Outline>();
            }
            // Set the outline width for the selected sphere
            selectedOutline.OutlineWidth = 5f; // You can set any width you want
            UpdateTextDisplay(selectedSphere.GetComponent<NodeComponent>().NodeData);
            OnLineToggleValueChanged(true);
        }
    }
    public void QuitTextDisplay()
    {
        // Reset the color and outline of the selected sphere when the Quit button (close button) is clicked
        if (selectedSphere != null)
        {
            Renderer selectedRenderer = selectedSphere.GetComponent<Renderer>();
            selectedRenderer.material.color = originalSphereColors[selectedSphere];
            // Reset the outline width
            if (selectedOutline != null)
            {
                selectedOutline.OutlineWidth = 0f;
            }
            lineToggle.isOn = false;
        }
        // Hide the text display
        displayObj.SetActive(false);
    }

    // toggle code, to only see lines directly connected to selected sphere

    void OnLineToggleValueChanged(bool isVisible)
    {
        if (!isVisible || selectedSphere == null)
        {
            foreach (LineRenderer lineRenderer in lines)
            {
                lineRenderer.enabled = !isVisible;
            }
        }
        else
        {   
            foreach (LineRenderer lineRenderer in lines)
            {   
                Vector3 startSpherePos = lineRenderer.GetPosition(0);
                Vector3 endSpherePos = lineRenderer.GetPosition(1);

                if (startSpherePos == selectedSphere.transform.position || endSpherePos == selectedSphere.transform.position)
                {   
                    lineRenderer.enabled = isVisible;
                }
                else
                {
                    lineRenderer.enabled = !isVisible;
                }
            }
        }

    }


    // searchbar code
    void SearchNode()
    {
        string nodeNameToSearch = searchInputField.text.Trim();
        string nodeNameToSearchCap = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(nodeNameToSearch);
        
        // Find and select the sphere corresponding to the entered node name
        GameObject foundSphere = FindSphere(nodeNameToSearchCap);
        if (foundSphere != null)
        {
            // Reset the color and outline of the previously selected sphere
            if (selectedSphere != null)
            {
                Renderer prevSelectedRenderer = selectedSphere.GetComponent<Renderer>();
                prevSelectedRenderer.material.color = originalSphereColors[selectedSphere];
                // Reset the outline width
                if (selectedOutline != null)
                {
                    selectedOutline.OutlineWidth = 0f;
                }
            }
            selectedSphere = foundSphere;
            Renderer newSelectedRenderer = selectedSphere.GetComponent<Renderer>();
            // Store the original color of the selected sphere
            if (!originalSphereColors.ContainsKey(selectedSphere))
            {
                originalSphereColors.Add(selectedSphere, newSelectedRenderer.material.color);
            }
            // Get or add the Outline component
            selectedOutline = selectedSphere.GetComponent<Outline>();
            if (selectedOutline == null)
            {
                selectedOutline = selectedSphere.AddComponent<Outline>();
            }

            // Set the outline width for the selected sphere
            selectedOutline.OutlineWidth = highlightSize; // You can set any width you want
            UpdateTextDisplay(selectedSphere.GetComponent<NodeComponent>().NodeData);
        }
        else
        {
            // Handle case when the entered node name is not found
            Debug.Log($"Node with name '{nodeNameToSearchCap}' not found.");
            StartCoroutine(ShowAndHide());
        }
    }
    IEnumerator ShowAndHide()
    {
        // Show the object
        WarningText.SetActive(true);
        // Wait for the specified display time
        yield return new WaitForSeconds(displayTime);
        // Hide the object after the display time has passed
        WarningText.SetActive(false);
    }
}
