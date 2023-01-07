using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node 
{
    [SerializeField] private int id;
    public int ID {
        get {return id;}
    }

    [SerializeField] List<int> connectedNodes = new List<int>();

    [SerializeField] Vector3S position;
    public Vector3 Position {
        get {return position.Back();}
    }

    [SerializeField] public List<int> streetsWithNode = new List<int>();

    public Node(Vector3 position, int ID, List<int> connectedNodes = null) {
        this.position = new Vector3S(position);
        id = ID;
        if (connectedNodes != null) this.connectedNodes = connectedNodes;
    }

    public void SetID(int newID) {
        id = newID;
    }

    public List<int> GetConnectedNodes() {
        return connectedNodes;
    }

    public bool AddConnectedNode(int nodeID) {
        if (!connectedNodes.Contains(nodeID)) {
            connectedNodes.Add(nodeID);
            return true;
        } 
        return false;
    }

    public bool RemoveConnectedNode(int nodeID) {
        if (!connectedNodes.Contains(nodeID)) {
            connectedNodes.Remove(nodeID);
            return true;
        }
        return false;
    }

    public int? GetNodeInDirection(Vector3 direction, List<Node> nodes) {
        foreach (int n in connectedNodes) {
            if ((nodes[n].Position - this.Position).normalized == direction.normalized) {
                return n;
            }
        }
        return null;
    }

    public int GetRandomConnectedNode() {
        return connectedNodes[Random.Range(0, connectedNodes.Count-1)];
    }

    public void SetPosition(Vector3 newPos) {
        position = new Vector3S(newPos);
    }

    public static float Distance(Node node1, Node node2) {
        return Vector3.Distance(node1.Position, node2.Position);
    }

    public static Vector3 Direction(Node node1, Node node2) {
        return node2.Position - node1.Position;
    }

    public static int? GetIDofPos(Vector3 pos, List<Node> nodes, List<int> nodesToSearch = null) {
        if (nodesToSearch != null) {
            foreach (int node in nodesToSearch) {
                if (nodes[node].position.Back() == pos) return nodes[node].ID;
            }
        } else {
            foreach (Node node in nodes) {
                if (node.position.Back() == pos) return node.ID;
            }
        }
        return null;
    }

    public static List<int> GetIDs(List<Node> nodes) {
        List<int> listToReturn = new List<int>();
        foreach (Node node in nodes) {
            listToReturn.Add(node.ID);
        }

        return listToReturn;
    }
}
