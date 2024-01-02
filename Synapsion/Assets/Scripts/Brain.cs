using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BrainObject : MonoBehaviour
{
    public Slider alphaSlider;
    public GameObject prefabVariant;
    // Reference to the shared material
    public Material sharedMaterial;

    // List of colors for each child object
    public List<Color> childColors = new List<Color> {
        new Color(0.992f, 1.0f, 0.714f, 0.13f),
        new Color(0.792f, 1.0f, 0.749f, 0.13f),
        new Color(0.992f, 1.0f, 0.714f, 0.13f),
        new Color(0.608f, 0.965f, 1.0f, 0.13f),
        new Color(0.992f, 1.0f, 0.714f, 0.13f),
        new Color(1.0f, 0.678f, 0.678f, 0.13f),
    };

    // For color blind friendly options
    public List<Color> colorblindFriendly = new List<Color> {
        Color.black,
        Color.white,
        Color.white,
        Color.white,
        Color.white,
        Color.white
    };

    private List<Color> originalColors = new List<Color> { };
    private float originalAlpha;

    // Color-blind friendly option
    public Toggle colorChange;

    void Start()
    {   
        // transparecy slider
        alphaSlider.onValueChanged.AddListener(OnAlphaSliderChanged);
        ChangeColor();

        // safe original color/transparecny
        originalColors = new List<Color>(childColors);
        originalAlpha = sharedMaterial.color.a;

        colorChange.onValueChanged.AddListener(ColorChange);
    }

    // Method that changes the actual color of the shared material 
    void ChangeColor()
    {
        // Apply colors to each child object
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            // Create a new material instance for each child
            Material childMaterial = new Material(sharedMaterial);

            // Set the color and original alpha for the child material
            childMaterial.color = new Color(childColors[i].r, childColors[i].g, childColors[i].b, originalAlpha);

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

    // Method that changes the albedo alpha value with the slider value
    void OnAlphaSliderChanged(float alphaValue)
    {
        // Store the new alpha value
        originalAlpha = alphaValue;

        // Iterate through the children of the prefab variant
        for (int j = 0; j < transform.childCount; j++)
        {
            Transform child = transform.GetChild(j);

            Renderer renderer = child.GetComponent<Renderer>();

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
                    currentColor.a = originalAlpha;
                    material.color = currentColor;
                }
            }
        }
    }

    // Method that changes the main color that will be used in the change color method
    public void ColorChange(bool isChanged)
    {
        if (isChanged)
        {
            childColors = new List<Color>(colorblindFriendly);
        }
        else
        {
            childColors = new List<Color>(originalColors);
        }
        ChangeColor();
    }
}
