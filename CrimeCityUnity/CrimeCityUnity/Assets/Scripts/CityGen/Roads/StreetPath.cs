using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StreetPath 
{
    List<int> nodeIDs = new List<int>();

    public List<int> NodeIDs {
        get {return nodeIDs;}
    }

    public StreetPath(List<int> nodeIDs) {
        this.nodeIDs = nodeIDs;
    }

    public static StreetPath GetRandomPath(int length, List<StreetNode> nodes, StreetNode startNode = null) {

        int start;
        int? previous = null;
        int current;
        List<int> path = new List<int>();

        if (startNode == null) start = Random.Range(0, nodes.Count-1); else start = startNode.ID;
        path.Add(start);
        current = start;

        for (int i = 0; i < length; i++) {
            
            List<int> options = new List<int>();
            foreach (int id in nodes[current].connectedNodeIDs) {
                if (id != previous) {
                    options.Add(id);
                }
            }

            if (options.Count > 0) {
                previous = current;
                current = options[Random.Range(0, options.Count-1)];
                path.Add(current);
            } else return new StreetPath (path);
        }

        return new StreetPath(path);
    }
}
