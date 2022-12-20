using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetMap
{
    public List<StreetNode> nodes = new List<StreetNode>();

    public StreetMap(List<KeyValuePair<Vector3, Vector3>> streets, List<Vector3> nodePositions) {

    }
}

public class StreetNode
{
    public Vector3 position;
    public List<int> connectedNodeIDs = new List<int>();

    public StreetNode(Vector3 position) {
        this.position = position;
    }

    public void AddConnection(int nodeID) {
        connectedNodeIDs.Add(nodeID);
    }

    public static float Distance(StreetNode startNode, StreetNode endNode) {
        return Vector3.Distance(startNode.position, endNode.position);
    }
}

public class StreetSection {
    StreetNode startNode;
    StreetNode endNode;

    public float Length {
        get {return StreetNode.Distance(startNode, endNode);}
    }

    public StreetSection(Vector3 start, Vector3 end) {
        startNode = new StreetNode(start);
        endNode = new StreetNode(end);
    }
}

