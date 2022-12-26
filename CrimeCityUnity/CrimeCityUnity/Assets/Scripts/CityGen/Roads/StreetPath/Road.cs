using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Road : StreetPath
{
    public int ID { get; private set; }
    public void SetID(int ID) {
        this.ID = ID;
    }

    public float speedLimit = 50;

    public Road(List<int> nodeIDs, List<Node> mapNodes) : base(nodeIDs, mapNodes) {
        SetNodeIDs(nodeIDs);
        SetLength(CalculateLength(mapNodes));
    }
}
