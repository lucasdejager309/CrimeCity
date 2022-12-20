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
}

[System.Serializable]
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

    public static List<StreetNode> Distinct(List<StreetNode> nodes) {
        List<StreetNode> nodesToRemove = new List<StreetNode>();
        
        for (int i = 0; i < nodes.Count; i++) {
            for (int j = 0; j < nodes.Count; j++) {
                if (i != j && SharesPosition(nodes[i], nodes[j])) {
                    nodesToRemove.Add(nodes[j]);
                }
            }
        }

        foreach (StreetNode node in nodesToRemove) {
            nodes.Remove(node);
        }

        return nodes;
    }
    
    public static bool SharesPosition(StreetNode node1, StreetNode node2) {
        if (node1.position == node2.position) return true;
        else return false;
    }

    public static bool ContainsPosition(List<StreetNode> nodes, Vector3 position) {
        foreach (StreetNode node in nodes) {
            if (node.position == position) return true;
        }
        return false;
    }
}

[System.Serializable]
public class StreetSection {
    public StreetNode startNode;
    public StreetNode endNode;

    public float Length {
        get {return StreetNode.Distance(startNode, endNode);}
    }

    public StreetSection(Vector3 start, Vector3 end) {
        startNode = new StreetNode(start);
        endNode = new StreetNode(end);
    }

    public static List<StreetSection> Distinct(List<StreetSection> sections) {
        List<StreetSection> sectionsToRemove = new List<StreetSection>();

        for (int i = 0; i < sections.Count; i++) {
            for (int j = 0; j < sections.Count; j++) {
                if (i != j && ComparePath(sections[i], sections[j]) && !StreetSection.ContainsPath(sectionsToRemove, sections[j])) {
                    sectionsToRemove.Add(sections[j]);
                }
            }
        }

        foreach (StreetSection section in sectionsToRemove) {
            sections.Remove(section);
        }

        return sections;
    }

    public static bool ComparePath(StreetSection section1, StreetSection section2) {
        if (
            (StreetNode.SharesPosition(section1.startNode, section2.startNode) && StreetNode.SharesPosition(section1.endNode, section2.endNode))
            ||
            (StreetNode.SharesPosition(section1.startNode, section2.endNode) && StreetNode.SharesPosition(section1.endNode, section2.startNode))
        ) {
            return true;
        } else return false;
    }

    public static bool ContainsPath(List<StreetSection> sections, StreetSection sectionToSearch) {
        foreach (StreetSection section in sections) {
            if (StreetSection.ComparePath(section, sectionToSearch)) return true;
        }
        return false;
    }
}

