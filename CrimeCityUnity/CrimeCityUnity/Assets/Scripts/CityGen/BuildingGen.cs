using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class BuildingGen {
    
    private class Bound {
        public float xMin = float.MaxValue;
        public float xMax = 0;
        public float zMin  = float.MaxValue;
        public float zMax = 0;

        public void Extend(float amount) {
            xMin -= amount;
            zMin -= amount;
            xMax += amount;
            zMax += amount;
        }

        public void Update(Node node) {
            if (node.Position.x < xMin) xMin = node.Position.x;
            if (node.Position.z < zMin) zMin = node.Position.z;
            if (node.Position.x > xMax) xMax = node.Position.x;
            if (node.Position.z > zMax) zMax = node.Position.z;
        }
    }
    public static List<Square> GetSquares(List<Node> nodes, float gridSize) {
        List<Square> squares = new List<Square>();

        //get bounds of city
        Bound bound = new Bound();
        foreach (Node node in nodes) {
            bound.Update(node);
        }
        //extend bounds 1 roadLength
        bound.Extend(gridSize*2);
        
        //create grid within these bounds
        for (float x = bound.xMin; x < bound.xMax; x += gridSize) {
            for (float z = bound.xMin; z < bound.xMax; z += gridSize) {
                Square square = new Square(new Vector3(x, 0, z), gridSize);
                 //if square in grid  has 2 or more connected streetnodes
                if (square.GetNodesOnGrid(nodes) != null) {
                    if (square.streetNodes.Count > 1) {
                        //add square in squaredictionary
                        squares.Add(square);
                    }
                }
            }
        }

        return squares;
    }
}

[System.Serializable]
public class Square {
    public Vector3S position {get; private set;}
    public Vector3 Position {
        get {return Vector3S.ConvertBack(position);}
    }
    private Vector3S centerPos = null;

    public Vector3S[] points {get; private set;} = new Vector3S[4];
    public List<int> streetNodes {get; private set;} = new List<int>();
    public void SetNodes(List<int> nodes) {
        streetNodes = nodes;
    }

    public Square(Vector3 newPosition, float gridSize) {
        position = new Vector3S(newPosition);
        
        points[0] = position;
        points[1] = new Vector3S(newPosition + new Vector3(gridSize, 0, 0));
        points[2] = new Vector3S(newPosition + new Vector3(gridSize, 0,  gridSize));
        points[3] = new Vector3S(newPosition + new Vector3(0, 0, gridSize));
    }

    public Vector3 GetCenterPos() {
        if (centerPos != null) return Vector3S.ConvertBack(centerPos);
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
        return Vector3S.ConvertBack(centerPos);
 }

    public List<int> GetNodesOnGrid(List<Node> nodes) {    
        int? possibleNode = null;
        foreach (Vector3S point in points) {
            possibleNode = Node.GetIDofPos(Vector3S.ConvertBack(point), nodes);
            if (possibleNode != null) {
                streetNodes.Add((int)possibleNode);
                break;
            }
        }

        if (possibleNode == null) return null; 
        else {
            foreach (int c in nodes[(int)possibleNode].GetConnectedNodes()) {
                foreach (Vector3S point in points) {
                    possibleNode = Node.GetIDofPos(Vector3S.ConvertBack(point), nodes);
                    if (possibleNode != null) {
                        if (!streetNodes.Contains((int)possibleNode)) streetNodes.Add((int)possibleNode);
                    }
                }
            }
        }

        return streetNodes;
    }
}