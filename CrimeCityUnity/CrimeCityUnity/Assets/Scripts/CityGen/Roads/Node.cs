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
        get {return Vector3S.ConvertBack(position);}
    }

    [SerializeField] public List<int> StreetsWithNode = new List<int>();

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

    public static int? GetIDofPos(Vector3 pos, List<Node> nodes) {
        foreach (Node node in nodes) {
            if (Vector3S.ConvertBack(node.position) == pos) return node.ID;
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
