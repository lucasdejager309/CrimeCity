using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PathNode {
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    public PathNode(int streetNodeID) {
        this.streetNodeID = streetNodeID;
    }

    public PathNode cameFromNode = null;
    public int streetNodeID;
    public float gCost = int.MaxValue;
    public float hCost;
    public float fCost {
        get {
            return gCost+hCost;
        }
    }

    public List<PathNode> GetNeighbouringNodes(List<Node> nodes) {
        List<PathNode> neighbours = new List<PathNode>();
        List<int> connectedNodes = nodes[streetNodeID].GetConnectedNodes();
        foreach (int n in connectedNodes) {
            neighbours.Add(new PathNode(n));
        }

        return neighbours;
	}

    public static float CalculateDistance(PathNode a, PathNode b, List<Node> nodes) {
        float xDistance = Mathf.Abs(nodes[a.streetNodeID].Position.x - nodes[b.streetNodeID].Position.x);
        float yDistance = Mathf.Abs(nodes[a.streetNodeID].Position.y - nodes[b.streetNodeID].Position.y);

        float remaining = Mathf.Abs(xDistance - yDistance);

        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    public static PathNode GetLowestFcostNode(List<PathNode> nodes) {
        PathNode lowestFCostNode = nodes[0];
        foreach (PathNode node in nodes) {
            if (node.fCost < lowestFCostNode.fCost) {
                lowestFCostNode = node;
            }
        }

        return lowestFCostNode;
    }

    public static bool ContainsID(List<PathNode> nodes, int id) {
        foreach (PathNode node in nodes) {
            if (node.streetNodeID == id) return true;
        }

        return false;
    }
}