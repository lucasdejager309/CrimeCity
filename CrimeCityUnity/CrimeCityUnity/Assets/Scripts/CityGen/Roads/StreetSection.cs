using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public bool ContainsNodePosition(Vector3 position) {
        if (startNode.position == position || endNode.position == position) {
            return true;
        }
        return false;
    }

    public StreetNode GetOtherNode(StreetNode node) {
        if (startNode.position == node.position) return endNode;
        else if (endNode.position == node.position) return startNode;
        else return null;
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
