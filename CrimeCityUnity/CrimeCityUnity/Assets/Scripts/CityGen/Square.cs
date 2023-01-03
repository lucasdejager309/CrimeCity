using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Square {
    private Vector3S centerPos = null;

    public Vector3S[] points {get; private set;} = new Vector3S[4];
    public Dictionary<int, int> streetNodes {get; private set;} = new Dictionary<int, int>();

    public Square(Vector3 newPosition, float gridSize) {
        
        points[0] = new Vector3S(newPosition);
        points[1] = new Vector3S(newPosition + new Vector3(gridSize, 0, 0));
        points[2] = new Vector3S(newPosition + new Vector3(gridSize, 0,  gridSize));
        points[3] = new Vector3S(newPosition + new Vector3(0, 0, gridSize));
    }

    public Vector3 GetCenterPos() {
        if (centerPos != null) return Vector3S.Back(centerPos);
        float x = 0f;
        float y = 0f;
        float z = 0f;
        foreach (Vector3S point in points)
        {
            x += point.x;
            y += point.y;
            z += point.z;
        }
        centerPos = new Vector3S(new Vector3(x / points.Length, y / points.Length, z / points.Length));
        return Vector3S.Back(centerPos);
    }

    public Dictionary<int, int> GetNodesOnGrid(List<Node> nodes) {    
        int? possibleNode = null;
        for (int i = 0; i < points.Length; i++) {
            possibleNode = Node.GetIDofPos(Vector3S.Back(points[i]), nodes);
            if (possibleNode != null) {
                streetNodes.Add(i, (int)possibleNode);
                break;
            }
        }

        if (possibleNode == null) return null; 
        else {
            foreach (int c in nodes[(int)possibleNode].GetConnectedNodes()) {
                for (int i = 0; i < points.Length; i++) {
                    possibleNode = Node.GetIDofPos(Vector3S.Back(points[i]), nodes);
                    if (possibleNode != null) {
                        if (!streetNodes.ContainsKey(i)) streetNodes.Add(i, (int)possibleNode);
                    }
                }
            }
        }

        return streetNodes;
    }


    public int GetzLayerDirection(List<Node> nodes) {
        int direction = 0;
        
        if (
            ( // 0, 1
                streetNodes.ContainsKey(0) && streetNodes.ContainsKey(1)
                &&
                nodes[streetNodes[0]].GetConnectedNodes().Contains(streetNodes[1])
            )
        ) {
            direction = 1;
        } else if (
            ( // 2, 3
                streetNodes.ContainsKey(2) && streetNodes.ContainsKey(3)
                &&
                nodes[streetNodes[2]].GetConnectedNodes().Contains(streetNodes[3])
            )
        ) {
            direction = -1;
        }

        return direction;
    }

    public int GetxLayerDirection(List<Node> nodes) {
        int direction = 0;

        if (
            ( // 0, 3
                streetNodes.ContainsKey(0) && streetNodes.ContainsKey(3)
                &&
                nodes[streetNodes[0]].GetConnectedNodes().Contains(streetNodes[3])
            )
        ) {
            direction = 1;
        } else if (
            ( // 1, 2
                streetNodes.ContainsKey(1) && streetNodes.ContainsKey(2)
                &&
                nodes[streetNodes[1]].GetConnectedNodes().Contains(streetNodes[2])
            )
        ) {

            direction = -1;
        }

        return direction;
    }
}