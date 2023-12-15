using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LegendController : MonoBehaviour
{
    // Reference to the legend GameObject
    public GameObject legendObject;

    // Call this method when the "Show Legend" button is pressed
    public void ShowLegend()
    {
        if (legendObject != null)
        {
            legendObject.SetActive(true);
        }
    }

    // Call this method when the "Close Legend" button is pressed
    public void CloseLegend()
    {
        if (legendObject != null)
        {
            legendObject.SetActive(false);
        }
    }
}
