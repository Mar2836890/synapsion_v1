using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NodeComponent : MonoBehaviour
{
    // This class can hold additional functionality related to nodes if needed
    // For now, it will store the Node data associated with the sphere
    //     public string name;


    public string Name;
    public string ParentName;
    public string Function;
    public List<string> ConnectedTo;
    public float XCoord;
    public float YCoord;
    public float ZCoord;
    public int NumEntry;

    public NetworkGenerator.Node NodeData { get; set; }

}