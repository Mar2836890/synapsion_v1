using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NodeDropdown : MonoBehaviour
{
    public TMP_Dropdown dropdown;  // Reference to the Dropdown UI component
    public SphereRandomGenerator sphereGenerator;  // Reference to the SphereRandomGenerator script

    void Start()
    {
        PopulateDropdown();
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    void PopulateDropdown()
    {   
        List<string> nodeNames = sphereGenerator.ListNodeNames;
        dropdown.ClearOptions();
        dropdown.AddOptions(nodeNames);

    }

    void OnDropdownValueChanged(int index)
    {
        // Get the selected node name from the dropdown
        string selectedNodeName = dropdown.options[index].text;
        // Call a method in SphereRandomGenerator to handle node selection
        sphereGenerator.SelectNodeByName(selectedNodeName);
    }
}
