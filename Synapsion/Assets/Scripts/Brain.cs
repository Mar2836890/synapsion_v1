using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainScript : MonoBehaviour
{   
    private GameObject structure;
    void Start()
    {
        // Set the position to (0, 0, 0) to center the model
        transform.position = Vector3.zero;
        // structure = new GameObject("Structure");
        // structure.AddComponent<RotateAndZoom>();
    }
}