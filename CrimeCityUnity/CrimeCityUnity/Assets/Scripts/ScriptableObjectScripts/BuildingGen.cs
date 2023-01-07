using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New BuildingGen", menuName = "BuildingGen")]
public class BuildingGen : ScriptableObject {
    public int layers = 1;
    public List<SpawnableBuilding> buildings = new List<SpawnableBuilding>();

    public BuildingMap GetMap(List<Node> nodes, float gridSize) {
        BuildingMap map = new BuildingMap(GetSquares(nodes, gridSize), gridSize);
        
        //temp
        foreach (SpawnableBuilding building in buildings) {
            map.AddBuilding(building);
        }

        return map;
    }

    public Dictionary<Vector3, Square> GetSquares(List<Node> nodes, float gridSize) {
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
                                    squares.Last().Value.GetNodesOnGrid(nodes);
                                    squares.Last().Value.GetzLayerDirection(nodes);
                                    squares.Last().Value.GetxLayerDirection(nodes);
                                }
                            } 

                            position = new Vector3(x, 0, z);
                            
                            if (xLayerDirection != 0) {
                                position = position + new Vector3((i*gridSize*xLayerDirection), 0, 0);
                                if (!squares.ContainsKey(position)) {
                                    squares.Add(position, new Square(position, gridSize));
                                    squares.Last().Value.GetNodesOnGrid(nodes);
                                    squares.Last().Value.GetzLayerDirection(nodes);
                                    squares.Last().Value.GetxLayerDirection(nodes);
                                }
                            }
                        }
                    }
                }
            }
        }

        squares = Square.GetConnectedSquares(squares, gridSize);

        return squares;
    }

}

public class Bound {
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