using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StreetMap
{
    public List<StreetSection> sections = new List<StreetSection>();
    public List<StreetNode> nodes = new List<StreetNode>();

    public StreetMap(List<StreetSection> sections, List<StreetNode> nodes) {
        this.sections = sections;
        this.nodes = nodes;
    }

    public List<StreetNode> GetRandomPath(int length) {
        StreetNode startNode = nodes[Random.Range(0, nodes.Count-1)];
        StreetNode lastNode = null;
        StreetNode currentNode = startNode;
        List<StreetNode> path = new List<StreetNode>();
        path.Add(currentNode);

        for (int i = 0; i < length; i++) {
            
            List<int> options = new List<int>();
            foreach (int id in currentNode.connectedNodeIDs) {
                if (nodes[id] != lastNode) {
                    options.Add(id);
                } 
            } 

            if (options.Count > 0) {
                lastNode = currentNode;
                currentNode = nodes[options[Random.Range(0, options.Count-1)]];
                path.Add(currentNode);
            } else return path;
        }

        return path;
    }
}

