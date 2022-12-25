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

    public StreetPath(List<int> nodeIDs) {
        this.nodeIDs = nodeIDs;
    }

    public bool ContainsNodes(List<int> nodes) {
        bool containsNodes = true;
        foreach (int n in nodes) {
            if (!nodeIDs.Contains(n)) containsNodes = false;
        }

        return containsNodes;
    }

    public static StreetPath GetStreet(StreetMap map, int start, Vector3? direction = null) {        
        List<int> pathNodes = new List<int>();
        List<int> usedNodes = new List<int>();
        Dictionary<int, List<int>> newPointsToGenerate = new Dictionary<int, List<int>>();
        
        int? previous = null;
        int current = map.Nodes[start].ID;
    
        while (true) {
            if (!pathNodes.Contains(current)) {
                pathNodes.Add(current);
            }
            
            //IF DIRECTION IS GIVEN FIND STRAIGHT CONNECTION
            if (direction == null) {
                direction = (map.Nodes[map.Nodes[current].GetRandomConnectedNode()].Position - map.Nodes[current].Position);
            }

            int? possibleNode = null;

            if (map.Nodes[current].GetNodeInDirection(((Vector3)direction), map.Nodes) != null) {
                //IF STRAIGHT CONNECTION
                possibleNode = map.Nodes[current].GetNodeInDirection(((Vector3)direction), map.Nodes);
            
            } else if (map.Nodes[current].GetConnectedNodes().Count == 2) {
                //IF NO STRAIGHT CONNECTION
                
                foreach (int n in map.Nodes[current].GetConnectedNodes()) {
                    if (previous != n) {
                        possibleNode = n;
                    }
                }
                
            } 

            if (possibleNode == null) {
                break;
            } else {
                previous = current;
                current = (int)possibleNode;
                direction = (map.Nodes[current].Position - map.Nodes[(int)previous].Position);
                pathNodes.Add((int)possibleNode);
                usedNodes.Add((int)possibleNode);
            }
        } 

        foreach (int n in pathNodes) {
            //Get unused connection nodes at intersections
            List<int> connectedNodesToAdd = new List<int>();
            foreach (int c in map.Nodes[current].GetConnectedNodes()) {
                if (!usedNodes.Contains(c)) {
                    connectedNodesToAdd.Add(c);
                }
            }
            if (!newPointsToGenerate.ContainsKey(current)) {
                newPointsToGenerate.Add(current, connectedNodesToAdd);
            } else {
                foreach (int i in connectedNodesToAdd) {
                    newPointsToGenerate[current].Add(i);
                }
            }
        }

        return new StreetPath(pathNodes);
    }
    
}
