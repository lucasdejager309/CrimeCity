using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodeConnection
{
    int id;
    public int ID {
        get {return id;}
    }
    int startID;
    public int StartID {
        get {return startID;}
    }
    int endID;
    public int EndID {
        get {return endID;}
    }
    Vector3S direction;
    public Vector3 Direction {
        get {return direction.Back();}
    }

    public NodeConnection(int id, int start, int end, List<Node> nodes) {
        this.id = id;
        startID = start;
        endID = end;
        direction = new Vector3S((nodes[end].Position - nodes[start].Position));
    }

    public int? GetOther(int id) {
        if (id == startID) return endID;
        if (id == endID) return startID;
        return null;
    }

    public bool ContainsNode(int id) {
        if (startID == id || endID == id) {
            return true;
        } return false;
    }

    public static List<int> GetConnectionsWith(int node, List<NodeConnection> connections) {
        List<int> connectionsToReturn = new List<int>();
        foreach(NodeConnection c in connections) {
            if (c.StartID == node || c.EndID == node) {
                connectionsToReturn.Add(c.ID);
            }
        }
        return connectionsToReturn;
    }

    public static int? GetConnectionInDirection(Vector3 direction, List<int> toCheck, List<NodeConnection> allConnections, bool absolute = false) {
        int? toReturn = null;
        foreach (int c in toCheck) {
            if (
                (allConnections[c].Direction.normalized == direction.normalized)
                ||
                (allConnections[c].Direction.normalized == -direction.normalized && absolute)
            ) {
                toReturn = c;
            }
        }
        return toReturn;
    }

    public static int? GetConnectionBetween(List<NodeConnection> connections, int startNode, int endNode) {
        foreach (var connection in connections) {
            if (
                (connection.StartID == startNode && connection.EndID == endNode)
                ||
                (connection.StartID == endNode && connection.EndID == startNode)
            ) {
                return connection.ID;
            }
        }
        return null;
    }

    // public static List<NodeConnection> GetConnectionsWithIDs(List<int> ids, List<NodeConnection> connections) {
    //     List<NodeConnection> connectionsToReturn = new List<NodeConnection>();
    //     foreach (var connection in connections) {
    //         if (ids.Contains(connection.ID)) {
    //             connectionsToReturn.Add(connection);
    //         }
    //     }

    //     return connectionsToReturn;
    // }
}
