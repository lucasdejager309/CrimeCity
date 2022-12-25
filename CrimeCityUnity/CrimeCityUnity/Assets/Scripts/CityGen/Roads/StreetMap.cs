using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StreetMap 
{
    List<NodeConnection> connections = new List<NodeConnection>();
    public List<NodeConnection> Connections {
        get {return connections;}
    }
    [SerializeField] List<Node> nodes = new List<Node>();
    public List<Node> Nodes {
        get {return nodes;}
    }
    [SerializeField] List<StreetPath> streets = new List<StreetPath>();
    public List<StreetPath> Streets {
        get {return streets;}
    }
    
    public StreetMap() {}

    public StreetMap(List<Node> nodes, List<NodeConnection> connections, bool generateStreets) {
        this.nodes = nodes;
        this.connections = connections;
        if (generateStreets) this.streets = StreetMap.GetStreets(this);
    }

    public static List<int> GetEnds(StreetMap map) {
        List<int> endIDs = new List<int>();
        foreach (Node node in map.Nodes) {
            if (node.GetConnectedNodes().Count == 1) endIDs.Add(node.ID);
        }
        return endIDs;
    }

    public static List<StreetPath> GetStreets(StreetMap map) {
        List<StreetPath> streets = new List<StreetPath>();
        // List<int> usedNodes = new List<int>();

        List<int> ends = StreetMap.GetEnds(map);
        List<int> ignore = new List<int>();
        foreach (int end in ends) {
            if (!ignore.Contains(end)) {
                StreetPath path = StreetPath.GetStreet(map, end);
                streets.Add(path);
                ignore.Add(map.nodes[path.NodeIDs.Count-1].ID);

                // foreach (int n in path.NodeIDs) {
                //     if (!usedNodes.Contains(n)) {
                //         usedNodes.Add(n);
                //     }
                // }
            }
        }
        
        return streets;
    }
}
