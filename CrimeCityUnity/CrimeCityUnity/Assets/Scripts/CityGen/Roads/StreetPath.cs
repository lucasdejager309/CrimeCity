using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StreetPath
{
    [SerializeField] List<int> nodeIDs = new List<int>();
    public List<int> NodeIDs {
        get {return nodeIDs;}
    }
    [SerializeField] List<int> connectionsIDs = new List<int>();
    public List<int> ConnectionsIDs {
        get {return connectionsIDs;}
    }

    public StreetPath(List<int> nodeIDs, List<int> connectionIDs) {
        this.nodeIDs = nodeIDs;
        this.connectionsIDs = connectionIDs;
    }

    public static StreetPath GetStreet(StreetMap map, int start, Vector3? direction = null) {        
        List<int> pathNodes = new List<int>();
        List<int> usedNodes = new List<int>();
        
        int? previous = null;
        int current = map.Nodes[start].ID;
    
        while (true) {
            if (!pathNodes.Contains(current)) {
                pathNodes.Add(current);
                usedNodes.Add(current);
            }
            int? possibleNode;
            //IF DIRECTION IS GIVEN FIND STRAIGHT CONNECTION
            if (direction == null) {
                possibleNode = map.Nodes[current].GetRandomConnectedNode();
            } else possibleNode = map.Nodes[current].GetNodeInDirection(((Vector3)direction), map.Nodes);


            if (possibleNode != null) {
                //IF STRAIGHT CONNECTION
                
                usedNodes.Add((int)possibleNode);
                previous = current;
                current = (int)possibleNode;
                direction = (map.Nodes[current].Position - map.Nodes[(int)previous].Position);
            } else if (map.Nodes[current].GetConnectedNodes().Count == 2) {
                //IF NO STRAIGHT CONNECTION
                
                foreach (int n in map.Nodes[current].GetConnectedNodes()) {
                    if (!usedNodes.Contains(n)) {
                        pathNodes.Add((int)n);

                        usedNodes.Add(n);
                        previous = current;
                        current = n;
                        direction = (map.Nodes[current].Position - map.Nodes[(int)previous].Position);
                    }
                }
                
            } else break;
        } 

        List<int> pathConnections = new List<int>();

        //TODO ADD CONNECTIONS

        // Debug.Log(pathNodes.Count + " " + pathConnections.Count);

        return new StreetPath(pathNodes, pathConnections);
    }
    
}
