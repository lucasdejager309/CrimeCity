using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Building {
    public int ID;
    public string buildingTypeID;
    public Vector3S position;
    public char direction;
    public List<Vector3S> squares = new List<Vector3S>();
    public Dictionary<Vector3S, int?> edgeNodes = new Dictionary<Vector3S, int?>();
    public Bound bound;

    public Building(string buildingTypeID, List<Vector3S> squares, BuildingMap map, int ID, Dictionary<Vector3S, int?> edgenodes, char direction) {
        this.ID = ID;
        this.buildingTypeID = buildingTypeID;
        this.position = squares[0];
        this.squares = squares;
        this.edgeNodes = edgenodes;
        this.direction = direction;

        bound = new Bound();
        foreach (Vector3S v in squares) {
            foreach (Vector3S point in map.Squares[v].points) {
                bound.Update(point.Back());
            }
        }
    }

    public static Dictionary<Vector3S, int?> GetEdgeNodes(BuildingMap map, List<Vector3S> squares, Bound bound = null) {
        Dictionary<Vector3S, int?> edgeNodes = new Dictionary<Vector3S, int?>();    

        if (bound == null) {
            bound = new Bound();
            foreach (Vector3S v in squares) {
                foreach (Vector3S point in map.Squares[v].points) {
                    bound.Update(point.Back());
                }
            }
        }

        // get points in squares that are on min or maxes;
        //also if streetNode is on point add node to dictionary
        foreach (Vector3S s in squares) {
            for (int i = 0; i < map.Squares[s].points.Length; i++) {
                if (
                    (map.Squares[s].points[i].x == bound.xMax || map.Squares[s].points[i].x == bound.xMin) 
                    ||
                    (map.Squares[s].points[i].z == bound.zMax || map.Squares[s].points[i].z == bound.zMin)
                ) {
                    int? streetNode = null; 
                    if (map.Squares[s].streetNodes.ContainsKey(i)) {
                        streetNode = map.Squares[s].streetNodes[i];
                    }

                    if (!edgeNodes.ContainsKey(map.Squares[s].points[i])) edgeNodes.Add(map.Squares[s].points[i], streetNode);   
                    else {
                        if (edgeNodes.ContainsKey(map.Squares[s].points[i]) && edgeNodes[map.Squares[s].points[i]] == null) {
                            edgeNodes[map.Squares[s].points[i]] = streetNode;
                        }
                    }                 
                }
            }
        }

        return edgeNodes;
    }

    //tries placing building randomly on map
    public static Building TryPlaceOnMap(BuildingMap map, SpawnableBuilding spawnableBuilding) {
        List<Vector3S> toTrySquares = map.squaresWithRoadAccess;
        while (true) {
            if (toTrySquares.Count == 0) return null;
            Vector3S randomSquare = toTrySquares[Random.Range(0, toTrySquares.Count)];
            toTrySquares.Remove(randomSquare);
            if (!map.takenSquares.Contains(randomSquare)) {
                Building building = FitsBuilding(map.Squares[randomSquare], spawnableBuilding, map);
                if (building != null) {
                    return building;
                }
            }
        }
    } 

    //checks if building fits on square in any rotation
    public static Building FitsBuilding(Square square, SpawnableBuilding spawnableBuilding, BuildingMap map) {
        List<Vector3S> buildingSquares = new List<Vector3S>();
        Dictionary<Vector3S, int?> edgeNodes = new Dictionary<Vector3S, int?>();

        Vector2Int direction;
        if (square.streetXDirections.Count == 1 && (square.streetZDirections.Count == 0 || square.streetZDirections.Count == 2)) {
            // |. or .| or [. or .]
            direction = new Vector2Int(-square.streetXDirections[0], 0);
        } else if ((square.streetXDirections.Count == 0 || square.streetXDirections.Count == 2) && square.streetZDirections.Count == 1) {
            // _ or - or |_| or |-|
            direction = new Vector2Int(0, -square.streetZDirections[0]);
        } else if (square.streetXDirections.Count == 1 && square.streetZDirections.Count == 1) {
            // |_ or |- or _| or -|
            direction = new Vector2Int(-square.streetXDirections[0], -square.streetZDirections[0]);
        } else return null;

        char cardinalDirection = 'N';
        if (direction.x != 0) {
            buildingSquares = GetBuildingSquares(square, map, spawnableBuilding.zSize, spawnableBuilding.xSize, direction);

            if (direction.x == 1) {
                cardinalDirection = 'W';
            } else cardinalDirection = 'E';

        } else if (direction.y != 0) {
            buildingSquares = GetBuildingSquares(square, map, spawnableBuilding.xSize, spawnableBuilding.zSize, direction);
            
            if (direction.y == -1) {
                cardinalDirection = 'S';
            }
        }
        if (buildingSquares == null) return null;

        //get edgenodes
        edgeNodes = Building.GetEdgeNodes(map, buildingSquares);
        //if node that is not edgenode contains street the building does not fit
        foreach (Vector3S s in buildingSquares) {
            for (int i = 0; i < map.Squares[s].points.Length; i++) {
                if (
                    !edgeNodes.ContainsKey(map.Squares[s].points[i]) 
                    &&
                    map.Squares[s].streetNodes.ContainsKey(i)
                ) {
                    return null;
                }
            }
        }
        
        //if all squares are allocated return squares, else return null (the building does not fit)
        return new Building(spawnableBuilding.buildingTypeID, buildingSquares, map, map.buildings.Count, edgeNodes, cardinalDirection);
    }

    private static List<Vector3S> GetBuildingSquares(Square square, BuildingMap map, int xSize, int zSize, Vector2Int direction) {
        List<Vector3S> buildingSquares = new List<Vector3S>();

        if (direction.x == 0) direction.x = 1;
        if (direction.y == 0) direction.y = 1;

        for (int xi = 0; xi < xSize; xi++) {
            for (int zi = 0; zi < zSize; zi++) {
                Vector3S currentPos = new Vector3S(square.position.x+(map.gridSize*xi*direction.x), 0, square.position.z+(map.gridSize*zi*direction.y));
                
                //if this is not the first square of the building, check if it is connected to other squares
                bool posIsConnected = true;
                if (buildingSquares.Count > 0) {
                    posIsConnected = false;
                    foreach(Vector3S sq in buildingSquares) {
                        if (map.Squares[sq].connectedSquares.Contains(currentPos)) {
                            posIsConnected = true;
                        }
                    }
                }

                //if this square is already taken by a different building OR square is not in map OR square is not connected: break
                if (
                    map.takenSquares.Contains(currentPos) 
                    || 
                    !map.Squares.ContainsKey(currentPos)
                    ||
                    !posIsConnected
                ) break;
                //else add it to building
                else buildingSquares.Add(currentPos);
            }
        }
        
        if (buildingSquares.Count < xSize*zSize) return null;
        return buildingSquares;
    }
}

