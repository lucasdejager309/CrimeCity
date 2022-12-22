using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : StreetPath {
    public float speedLimit;

    public Road(List<int> IDs) {
        nodeIDs = IDs;
    }
}
