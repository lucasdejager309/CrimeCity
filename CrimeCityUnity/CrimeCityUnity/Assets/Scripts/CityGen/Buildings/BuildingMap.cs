using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildingMap {
    
    Dictionary<Vector3S, Square> squares = new Dictionary<Vector3S, Square>();
    public Dictionary<Vector3S, Square> Squares {
        get {return squares;}
    }
    public float gridSize {get; private set;}
    public List<Vector3S> squaresWithRoadAccess {get; private set;} = new List<Vector3S>();
    [SerializeField] List<Building> buildings = new List<Building>();

    public BuildingMap(Dictionary<Vector3, Square> squares, float gridSize) {
        SetSquares(squares);
        this.gridSize = gridSize;
    }

    private void SetSquares(Dictionary<Vector3, Square> squares) {
        foreach (var entry in squares) {
            this.squares.Add(new Vector3S(entry.Key), entry.Value);
            if (entry.Value.streetNodes.Count > 1) {
                squaresWithRoadAccess.Add(new Vector3S(entry.Key));
            }
        }
    }

    public void AddBuilding(SpawnableBuilding building, Vector3? position = null) {
        List<Vector3S> buildingSquares = Building.GetPossiblePosition(this, building, position);
        if (buildingSquares != null) {
            Debug.Log(building.buildingTypeID + " " + buildingSquares[0].ToString() + " " + buildingSquares.Count);
            buildings.Add(new Building(building.buildingTypeID, buildingSquares[0].Back(), buildings.Count));
        }
    }
}
