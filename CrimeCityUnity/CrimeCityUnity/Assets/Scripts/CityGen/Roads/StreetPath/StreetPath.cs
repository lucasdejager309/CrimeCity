using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StreetPath
{
    [SerializeField] List<int> nodeIDs = new List<int>();
    public List<int> NodeIDs {
        get {return nodeIDs;}
    }
    public void SetNodeIDs(List<int> nodeIDs) {
        this.nodeIDs = nodeIDs;
    }
    public float length { get; private set; }
    public void SetLength(float length) {
        this.length = length;
    } 



    public StreetPath(List<int> nodeIDs, List<Node> mapNodes) {
        SetNodeIDs(nodeIDs);
        if (NodeIDs.Count != 0) SetLength(CalculateLength(mapNodes));
    }

    public bool ContainsNodes(List<int> nodes) {
        bool containsNodes = true;
        foreach (int n in nodes) {
            if (!nodeIDs.Contains(n)) containsNodes = false;
        }

        return containsNodes;
    }

    public float CalculateLength(List<Node> nodes) {
            float length = 0;

            int? previous = null;
            int current = nodeIDs[0];

            foreach (int n in nodeIDs) {
                previous = current;
                current = n;
                length += Vector3.Distance(nodes[current].Position, nodes[(int)previous].Position);
            }

            return length;
    }
    
}
