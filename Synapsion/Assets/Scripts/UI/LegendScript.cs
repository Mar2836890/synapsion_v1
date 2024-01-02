using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendController : MonoBehaviour
{
    // Ref to the legend 
    public GameObject legendObject;

    // Call this method when the Toggle button is pressed
    public void ToggleLegend()
    {
        if (legendObject != null)
        {
            // Check if the legend is currently active
            bool isActive = legendObject.activeSelf;

            // Toggle the visibility of the legend
            legendObject.SetActive(!isActive);
        }
    }
}
