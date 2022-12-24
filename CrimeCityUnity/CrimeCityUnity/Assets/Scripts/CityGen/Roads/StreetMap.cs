using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StreetMap 
{
    [SerializeField] List<StreetNode> nodes = new List<StreetNode>();
    public List<StreetNode> Nodes {
        get {return nodes;}
    }

    public StreetMap() {}

    public StreetMap(List<StreetNode> nodes) {
        this.nodes = nodes;
    }
}
