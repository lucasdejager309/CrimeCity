using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New BuildingGen", menuName = "BuildingGen")]
public class BuildingGen : ScriptableObject {
    public int layers = 1;

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
    public Dictionary<Vector3S, Square> GetSquares(List<Node> nodes, float gridSize) {
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
                    if (square.streetNodes.Count >= 2) {
                        //add square in squaredictionary
                        Vector3 position = new Vector3(x, 0, z);
                        int zLayerDirection = square.GetzLayerDirection(nodes);
                        int xLayerDirection = square.GetxLayerDirection(nodes);

                        for (int i = 0; i < layers; i++) {
                            position = new Vector3(x, 0, z);
                            
                            if (zLayerDirection != 0) {
                                position = position + new Vector3(0, 0, (i*gridSize*zLayerDirection));
                                if (!squares.ContainsKey(position)) {
                                    squares.Add(position, new Square(position, gridSize));
                                }
                            } 

                            position = new Vector3(x, 0, z);
                            
                            if (xLayerDirection != 0) {
                                position = position + new Vector3((i*gridSize*xLayerDirection), 0, 0);
                                if (!squares.ContainsKey(position)) {
                                    squares.Add(position, new Square(position, gridSize));
                                }
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