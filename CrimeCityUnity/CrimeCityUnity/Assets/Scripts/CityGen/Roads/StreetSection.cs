using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StreetSection {
    public StreetMap parentMap;
    public int startID;
    public int endID;

    public float Length {
        get {return Vector3.Distance(parentMap.nodes[startID].position, parentMap.nodes[endID].position);}
    }
    
    public StreetNode StartNode {
        get {return parentMap.nodes[startID];}
    }

    public StreetNode EndNode {
        get {return parentMap.nodes[endID];}
    }

    public StreetSection(Vector3 start, Vector3 end, StreetMap parentMap) {
        this.parentMap = parentMap;
        for (int i = 0; i < parentMap.nodes.Count; i++) {
            if (parentMap.nodes[i].position == start) startID = i;
            if (parentMap.nodes[i].position == end) endID = i;
        }
    }

    public bool ContainsNodePosition(Vector3 position) {
        if (parentMap.nodes[startID].position == position || parentMap.nodes[endID].position == position) {
            return true;
        }
        return false;
    }

    public Vector3 GetOtherPosition (Vector3 position) {
        if (parentMap.nodes[startID].position == position) return parentMap.nodes[endID].position;
        else return parentMap.nodes[startID].position;
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
            (
                section1.StartNode.position == section2.StartNode.position 
                && 
                section1.EndNode.position == section2.EndNode.position
            )
            ||
            (
                section1.StartNode.position == section2.EndNode.position 
                && 
                section1.EndNode.position == section2.StartNode.position
            )
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
