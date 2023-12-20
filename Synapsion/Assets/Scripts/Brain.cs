using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AlphaSlider : MonoBehaviour
{
    public Slider alphaSlider;
    public GameObject prefabVariant;
    // Reference to the shared material
    public Material sharedMaterial;

    // List of colors for each child object
    // 1 yellow, 2 green, 3 yellow, 4 red, 5 yellow, 6 blue
    public List<Color> childColors = new List<Color> {
        new Color(0.992f, 1.0f, 0.714f, 0.13f),
        new Color(0.792f, 1.0f, 0.749f, 0.13f),
        new Color(0.992f, 1.0f, 0.714f, 0.13f),
        new Color(0.608f, 0.965f, 1.0f, 0.13f),
        new Color(0.992f, 1.0f, 0.714f, 0.13f),
        new Color(1.0f, 0.678f, 0.678f, 0.13f),
    };

    // For color blind friendly button
    public List<Color> colorblindFriendly = new List<Color> {
        Color.black,
        Color.white,
        Color.white,
        Color.white,
        Color.white,
        Color.white
    };

    public List<Color> OriginalColors = new List<Color>{};
    public Toggle colorChange;

    void Start()
    {
        // Add a listener to the slider to respond to changes
        alphaSlider.onValueChanged.AddListener(OnAlphaSliderChanged);
        ChangeColor();

        OriginalColors = childColors;
        colorChange.onValueChanged.AddListener(ColorChange);
    }

    void ChangeColor() // Removed semicolon here
    {
        if (childColors.Count != transform.childCount)
        {
            Debug.LogError("Number of child colors does not match the number of children.");
            return;
        }

        // Apply colors to each child object
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            // Create a new material instance for each child
            Material childMaterial = new Material(sharedMaterial);

            // Set the color for the child material
            childMaterial.color = childColors[i];

            // Apply the material to the child object
            Renderer renderer = child.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = childMaterial;
            }
            else
            {
                Debug.LogWarning("Child object does not have a Renderer component.");
            }
        }

    }

    void OnAlphaSliderChanged(float alphaValue)
    {
        // Iterate through the children of the prefab variant
        for (int j = 0; j < transform.childCount; j++)
        {   
            Transform child = transform.GetChild(j);

            Renderer renderer = child.GetComponent<Renderer>();

            // Check if the object has a Renderer component
            if (renderer != null)
            {
                // Use sharedMaterials instead of materials
                Material[] materials = renderer.sharedMaterials;

                // Iterate through the materials of the Renderer
                for (int i = 0; i < materials.Length; i++)
                {
                    // Get the current material
                    Material material = materials[i];

                    // Set the alpha value of the material's color
                    Color currentColor = material.color;
                    currentColor.a = alphaValue;
                    material.color = currentColor;
                }
            }
        }
    }
    public void ColorChange(bool isCanged)
    {
        if(!isCanged)
        {
            childColors = OriginalColors;
        }
        else
        {
            childColors = colorblindFriendly;
        }
        ChangeColor();
    }
}
