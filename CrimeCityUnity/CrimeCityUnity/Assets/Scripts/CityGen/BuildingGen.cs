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
            if (node.Position.x < xMin) {xMin = node.Position.x;}
            if (node.Position.z < zMin) {zMin = node.Position.z;}
            if (node.Position.x > xMax) {xMax = node.Position.x;}
            if (node.Position.z > zMax) {zMax = node.Position.z;}
        }
    }
    public static Dictionary<Vector3S, Square> GetSquares(List<Node> nodes, float gridSize, int layers) {
        Dictionary<Vector3, Square> squares = new Dictionary<Vector3, Square>();

        //get bounds of city
        Bound bound = new Bound();
        foreach (Node node in nodes) {
            bound.Update(node);
        }
        //extend bounds 1 roadLength
        bound.Extend(gridSize);

        //create grid within these bounds
        for (float x = bound.xMin; x < bound.xMax; x += gridSize) {
            for (float z = bound.zMin; z < bound.zMax; z += gridSize) {
                Square square = new Square(new Vector3(x, 0, z), gridSize);
                 //if square in grid  has 2 or more connected streetnodes
                if (square.GetNodesOnGrid(nodes) != null) {
                    if (square.streetNodes.Count > 1) {
                        //add square in squaredictionary
                        Vector3 position = new Vector3(x, 0, z);
                        if (!squares.ContainsKey(position)) squares.Add(position, square);
                        
                        int zLayerDirection = square.GetzLayerDirection(nodes);
                        if (zLayerDirection != 0) {
                            position = position + new Vector3(0, 0, (gridSize*zLayerDirection));
                            if (!squares.ContainsKey(position)) {
                                squares.Add(position, new Square(position, gridSize));
                            }
                        }
                        int xLayerDirection = square.GetxLayerDirection(nodes);
                        if (xLayerDirection != 0) {
                            position = position + new Vector3((gridSize*xLayerDirection), 0, 0);
                            if (!squares.ContainsKey(position)) {
                                squares.Add(position, new Square(position, gridSize));
                            }
                        }
                        
                    }
                }
            }
        }

        Dictionary<Vector3S, Square> squaresS = new Dictionary<Vector3S, Square>();
        foreach (var kv in squares) {
            squaresS.Add(new Vector3S(kv.Key), kv.Value);
        }

        return squaresS;
    }
}

[System.Serializable]
public class Square {
    private Vector3S centerPos = null;

    public Vector3S[] points {get; private set;} = new Vector3S[4];
    public List<int> streetNodes {get; private set;} = new List<int>();
    public void SetNodes(List<int> nodes) {
        streetNodes = nodes;
    }

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

    public List<int> GetNodesOnGrid(List<Node> nodes) {    
        int? possibleNode = null;
        foreach (Vector3S point in points) {
            possibleNode = Node.GetIDofPos(Vector3S.Back(point), nodes);
            if (possibleNode != null) {
                streetNodes.Add((int)possibleNode);
                break;
            }
        }

        if (possibleNode == null) return null; 
        else {
            foreach (int c in nodes[(int)possibleNode].GetConnectedNodes()) {
                foreach (Vector3S point in points) {
                    possibleNode = Node.GetIDofPos(Vector3S.Back(point), nodes);
                    if (possibleNode != null) {
                        if (!streetNodes.Contains((int)possibleNode)) streetNodes.Add((int)possibleNode);
                    }
                }
            }
        }

        return streetNodes;
    }


    public int GetzLayerDirection(List<Node> nodes) {
        int direction = 0;

        if (streetNodes.Count != 2) return direction;
        
        if (
            (
                Node.GetIDofPos(points[0].Back(), nodes, streetNodes) != null
                &&
                Node.GetIDofPos(points[1].Back(), nodes, streetNodes) != null
            )
        ) {
            direction = 1;
        } else if (
            (
                Node.GetIDofPos(points[2].Back(), nodes, streetNodes) != null
                &&
                Node.GetIDofPos(points[3].Back(), nodes, streetNodes) != null
            )
        ) {
            direction = -1;
        }

        return direction;
    }

    public int GetxLayerDirection(List<Node> nodes) {
        int direction = 0;

        if (
            (
                Node.GetIDofPos(points[0].Back(), nodes, streetNodes) != null
                &&
                Node.GetIDofPos(points[3].Back(), nodes, streetNodes) != null
            )
        ) {
            direction = 1;
        } else if (
            (
                Node.GetIDofPos(points[1].Back(), nodes, streetNodes) != null
                &&
                Node.GetIDofPos(points[2].Back(), nodes, streetNodes) != null
            )
        ) {
            direction = -1;
        }

        return direction;
    }
}