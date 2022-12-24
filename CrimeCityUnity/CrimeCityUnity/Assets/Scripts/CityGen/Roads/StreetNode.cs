using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StreetNode 
{
    [SerializeField] private int id;
    public int ID {
        get {return id;}
    }

    [SerializeField] List<int> connectedNodes = new List<int>();

    [SerializeField] Vector3 position;
    public Vector3 Position {
        get {return position;}
    }

    public StreetNode(Vector3 position, int ID, List<int> connectedNodes = null) {
        this.position = position;
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

    public int GetRandomConnectedNode() {
        return connectedNodes[Random.Range(0, connectedNodes.Count-1)];
    }

    public void SetPosition(Vector3 newPos) {
        position = newPos;
    }

    public static int? GetIDofPos(Vector3 pos, List<StreetNode> nodes) {
        foreach (StreetNode node in nodes) {
            if (node.position == pos) return node.ID;
        }
        return null;
    }
}
