using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public class Node : MonoBehaviour
// {
//     public string name;
//     public int id;
//     public int x;
//     public int y;
//     public int z;
//     public List<Node> children;
// }

public class NodeComponent : MonoBehaviour
{
    // This class can hold additional functionality related to nodes if needed
    // For now, it will store the Node data associated with the sphere
    //     public string name;


    public string Name;
    public string Function;
    public List<string> ConnectedTo;
    public int XCoord;
    public int YCoord;
    public int ZCoord;

    public SphereRandomGenerator.Node NodeData { get; set; }

}