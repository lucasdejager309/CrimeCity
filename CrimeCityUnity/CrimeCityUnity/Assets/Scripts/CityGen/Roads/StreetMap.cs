using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    // public static List<int> GetEnds(StreetMap map) {
    //     List<int> endIDs = new List<int>();
    //     foreach (Node node in map.Nodes) {
    //         if (node.GetConnectedNodes().Count == 1) endIDs.Add(node.ID);
    //     }
    //     return endIDs;
    // }

    public static List<StreetPath> GetStreets(StreetMap map) {
        List<StreetPath> streets = new List<StreetPath>();
        
        //find dead ends
        List<int> deadEnds = new List<int>();
        //find t sections
        List<int> tSections = new List<int>();
        foreach(Node n in map.nodes) {
            if (n.GetConnectedNodes().Count == 1) deadEnds.Add(n.ID);
            if (n.GetConnectedNodes().Count == 3) tSections.Add(n.ID);
        }

        List<int> ignore = new List<int>();
        foreach (int n in deadEnds) {
            if (!ignore.Contains(n)) {
                StreetPath path = StreetPath.GetStreet(map, n);
                ignore.Add(path.NodeIDs.Last());
                streets.Add(path);
            }
        }

        foreach (int n in tSections) {
            if (!ignore.Contains(n)) {
                Vector3 direction = new Vector3();
                foreach(int c in map.nodes[n].GetConnectedNodes()) {
                    direction += map.nodes[c].Position - map.nodes[n].Position;
                }
                direction = direction.normalized;

                StreetPath path = StreetPath.GetStreet(map, n, direction);
                ignore.Add(path.NodeIDs.Last());
                streets.Add(path);
            }
        }
        
        return streets;
    }
}
