using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StreetSection {
    public int startID;
    public int endID;

    public float Length(StreetMap map) {
        return Vector3.Distance(StartNode(map).position, EndNode(map).position);
    }
    
    public StreetNode StartNode(StreetMap parentMap) {
        return parentMap.nodes[startID];
    }

    public StreetNode EndNode(StreetMap parentMap) {
        return parentMap.nodes[endID];
    }

    public StreetSection(Vector3 start, Vector3 end, StreetMap parentMap) {
        for (int i = 0; i < parentMap.nodes.Count; i++) {
            if (parentMap.nodes[i].position == start) startID = i;
            if (parentMap.nodes[i].position == end) endID = i;
        }
    }

    public bool ContainsNodePosition(Vector3 position, StreetMap map) {
        if (StartNode(map).position == position || EndNode(map).position == position) {
            return true;
        }
        return false;
    }

    public Vector3 GetOtherPosition (Vector3 position, StreetMap map) {
        if (StartNode(map).position == position) return EndNode(map).position;
        else return StartNode(map).position;
    }

    public static bool ComparePath(StreetSection section1, StreetSection section2, StreetMap map) {
        if (
            (
                section1.StartNode(map).position == section2.StartNode(map).position 
                && 
                section1.EndNode(map).position == section2.EndNode(map).position
            )
            ||
            (
                section1.StartNode(map).position == section2.EndNode(map).position 
                && 
                section1.EndNode(map).position == section2.StartNode(map).position
            )
        ) {
            return true;
        } else return false;
    }

    public static bool ContainsPath(List<StreetSection> sections, StreetSection sectionToSearch, StreetMap map) {
        foreach (StreetSection section in sections) {
            if (StreetSection.ComparePath(section, sectionToSearch, map)) return true;
        }
        return false;
    }
}
