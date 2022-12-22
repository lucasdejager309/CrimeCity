using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StreetNode
{
    public int ID {get; private set;}

    public Vector3 position;
    public List<int> connectedNodeIDs = new List<int>();

    public StreetNode(Vector3 position, int ID, List<int> connectedNodeIDs = null) {
        this.position = position;
        if (connectedNodeIDs != null) this.connectedNodeIDs = connectedNodeIDs;
        this.ID = ID;
    }

    public bool AddConnection(Vector3 positionToAdd, List<StreetNode> nodes) {
        for (int i = 0; i < nodes.Count; i++) {
            if (nodes[i].position == positionToAdd) {
                if (!connectedNodeIDs.Contains(i)) connectedNodeIDs.Add(i);
                return true;
            }
        }
        return false;
    }

    public static float Distance(StreetNode startNode, StreetNode endNode) {
        return Vector3.Distance(startNode.position, endNode.position);
    }

    public static bool ContainsPosition(List<StreetNode> nodes, Vector3 position) {
        foreach (StreetNode node in nodes) {
            if (node.position == position) return true;
        }
        return false;
    }

    public static void DebugNodes(List<StreetNode> nodes) {
        for (int i = 0; i < nodes.Count; i++) {
            Debug.Log("ID: " + i + "\npos: " + nodes[i].position + "\n connections: " + nodes[i].connectedNodeIDs.Count);
        }
    }
}
