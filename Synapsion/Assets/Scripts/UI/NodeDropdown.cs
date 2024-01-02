using System.Collections.Generic;
using System.Linq; 
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NodeDropdown : MonoBehaviour
{
    public TMP_Dropdown dropdown; 

    // Ref to the NetworkGenerator script
    public NetworkGenerator sphereGenerator;  

    void Start()
    {
        PopulateDropdown();
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    void PopulateDropdown()
    {   
        List<string> nodeNames = sphereGenerator.ListNodeNames;

        // Sort the list alphabetically
        nodeNames = nodeNames.OrderBy(name => name).ToList();

        dropdown.ClearOptions();
        dropdown.AddOptions(nodeNames);
    }

    void OnDropdownValueChanged(int index)
    {
        // Get the selected node name from the dropdown
        string selectedNodeName = dropdown.options[index].text;
        // Call a method in NetworkGenerator to handle node selection
        sphereGenerator.SelectNodeByName(selectedNodeName);
        sphereGenerator.searchInputField.text = selectedNodeName;
    }
}
