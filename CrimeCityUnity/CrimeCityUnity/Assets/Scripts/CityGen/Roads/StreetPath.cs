using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StreetPath 
{
    public List<int> nodeIDs = new List<int>();

    public StreetPath() {}

    public StreetPath(List<int> nodeIDs) {
        this.nodeIDs = nodeIDs;
    }

    // public static StreetPath GetRandomPath(int length, List<StreetNode> nodes, StreetNode startNode = null) {

    //     int start;
    //     int? previous = null;
    //     int? current = null;
    //     List<int> path = new List<int>();

    //     if (startNode == null) start = Random.Range(0, nodes.Count-1); else start = startNode.ID;
    //     path.Add(start);
    //     current = start;

    //     for (int i = 0; i < length; i++) {
            
    //             previous = (int)current;
    //             current = nodes[(int)nodes[(int)current].GetRandomConnection(previous)].ID;
    //             if (current != null)
    //             path.Add((int)current);
    //             else return new StreetPath(path);
    //     }

    //     return new StreetPath(path);
    // }
}
