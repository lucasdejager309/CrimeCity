using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnableBuilding
{
    public string buildingTypeID;
    public int xSize;
    public int zSize;

    public GameObject prefab;

}

[System.Serializable]
public class Building {
    public int ID;
    public string buildingTypeID;
    public Vector3S position;
    public List<Vector3S> squares = new List<Vector3S>();
    public Dictionary<Vector3S, int?> edgeNodes = new Dictionary<Vector3S, int?>();
    public Bound bound;

    public Building(string buildingTypeID, List<Vector3S> squares, BuildingMap map, int ID) {
        this.ID = ID;
        this.buildingTypeID = buildingTypeID;
        this.position = squares[0];
        this.squares = squares;
        SetEdgeNodes(map);
    }

    private void SetEdgeNodes(BuildingMap map) {
        
        bound = new Bound();

        foreach (Vector3S v in squares) {
            foreach (Vector3S point in map.Squares[v].points) {
                bound.Update(point.Back());
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

        //order them somehow?
        //maybe not needed?
    }

    public static List<Vector3S> GetPossiblePosition(BuildingMap map, SpawnableBuilding building, Vector3? position = null, float maxDistanceFromPos = 0) {
        if (position == null) {
            foreach (Vector3S positionS in map.squaresWithRoadAccess) {
                if (map.takenSquares.Contains(positionS)) continue;
                
                List<Vector3S> squares = map.Squares[positionS].FitsBuilding(building, map);
                if (squares != null) {
                    return squares;
                }
            }
        } else {
            //find possible position closest to given position...
        }

        return null;
    } 
}
