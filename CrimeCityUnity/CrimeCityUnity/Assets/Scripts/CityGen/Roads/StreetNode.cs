using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StreetNode
{
    public Vector3 position;
    public List<int> connectedNodeIDs = new List<int>();

    public StreetNode(Vector3 position) {
        this.position = position;
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

    public static int GetID(Vector3 pos, List<StreetNode> nodes) {
        for (int i = 0; i < nodes.Count; i++) {
            if (nodes[i].position == pos) {
                return i;
            }
        }

        return nodes.Count+1;
    }

    public static void DebugNodes(List<StreetNode> nodes) {
        for (int i = 0; i < nodes.Count; i++) {
            Debug.Log("ID: " + i + "\npos: " + nodes[i].position + "\n connections: " + nodes[i].connectedNodeIDs.Count);
        }
    }
}
