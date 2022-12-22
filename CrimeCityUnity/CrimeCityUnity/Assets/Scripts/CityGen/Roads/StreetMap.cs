using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StreetMap
{
    public List<StreetPath> streets = new List<StreetPath>();
    public List<StreetSection> sections = new List<StreetSection>();
    public List<StreetNode> nodes = new List<StreetNode>();

    public StreetMap(List<StreetSection> sections = null, List<StreetNode> nodes = null, bool createStreets = true, List<StreetPath> streets = null) {
        if (sections != null) this.sections = sections; else sections = new List<StreetSection>();
        if (nodes != null) this.nodes = nodes; else nodes = new List<StreetNode>();
        if (streets != null) this.streets = streets; else if (createStreets) streets = StreetMap.CreateStreets(sections, nodes);
    }

    public static StreetMap LoadMap(Save save) {
        List<StreetNode> nodesToReturn = new List<StreetNode>();
        foreach(var kv in save.nodes) {
            nodesToReturn.Add(new StreetNode(Vector3S.ConvertBack(kv.Key), nodesToReturn.Count, kv.Value));
        }
        
        StreetMap map = new StreetMap(save.streetSections, nodesToReturn, false, save.streets);
        return map;
    }

    public static List<StreetPath> CreateStreets(List<StreetSection> sections, List<StreetNode> nodes) {
        
        return null;
    }
}

