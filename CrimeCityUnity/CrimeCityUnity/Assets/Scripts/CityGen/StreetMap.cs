using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class StreetMap 
{
    [SerializeField] List<Node> nodes = new List<Node>();
    public List<Node> Nodes {
        get {return nodes;}
    }
    [SerializeField] List<Road> streets = new List<Road>();
    public List<Road> Streets {
        get {return streets;}
    }
    
    public StreetMap() {}

    public StreetMap(List<Node> nodes, List<Road> streets = null) {
        this.nodes = nodes;
        if (streets == null) this.streets = StreetMap.GetStreets(this);
    }

    public static List<Road> GetStreets(StreetMap map) {
        List<Road> streets = new List<Road>();
        
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
                Road road = Road.GetStreet(map, n);
                ignore.Add(road.NodeIDs.Last());
                streets.Add(road);
            }
        }

        foreach (int n in tSections) {
            if (!ignore.Contains(n)) {
                Vector3 direction = new Vector3();
                foreach(int c in map.nodes[n].GetConnectedNodes()) {
                    direction += map.nodes[c].Position - map.nodes[n].Position;
                }
                direction = direction.normalized;

                Road road = Road.GetStreet(map, n, direction);
                ignore.Add(road.NodeIDs.Last());
                streets.Add(road);
            }
        }

        for(int i = 0; i < streets.Count; i++) {
            streets[i].SetID(i);

            foreach (int n in streets[i].NodeIDs) {
                map.nodes[n].streetsWithNode.Add(streets[i].ID);
            }
        }
        
        return streets;
    }
}
