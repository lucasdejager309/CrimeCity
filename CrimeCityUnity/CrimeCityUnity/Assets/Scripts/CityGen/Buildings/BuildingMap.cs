using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class BuildingMap {
    
    public float gridSize {get; private set;}
    
    //squares
    Dictionary<Vector3S, Square> squares = new Dictionary<Vector3S, Square>();
    public Dictionary<Vector3S, Square> Squares {
        get {return squares;}
    }
    public List<Vector3S> takenSquares = new List<Vector3S>();
    public List<Vector3S> squaresWithRoadAccess {get; private set;} = new List<Vector3S>();

    public List<SquareGroup> groups {get; private set;} = new List<SquareGroup>();

    //buildings
    public List<Building> buildings = new List<Building>();

    Dictionary<string, int> buildingAmounts = new Dictionary<string, int>();
    private void AddToCount(Building building) {
        if (buildingAmounts.ContainsKey(building.buildingTypeID)) buildingAmounts[building.buildingTypeID]++;
        else buildingAmounts.Add(building.buildingTypeID, 1);
    }
    private bool CheckBuildingAmount(string ID, int amount) {
        if (!buildingAmounts.ContainsKey(ID)) return true;
        else if (buildingAmounts[ID] < amount) return true;
        return false;
    }

    public BuildingMap(Dictionary<Vector3, Square> squares, float gridSize) {
        SetSquares(squares);
        this.gridSize = gridSize;
    }

    public BuildingMap() {}

    private void SetSquares(Dictionary<Vector3, Square> squares) {
        foreach (var entry in squares) {
            this.squares.Add(new Vector3S(entry.Key), entry.Value);
            if (entry.Value.streetNodes.Count > 1) {
                squaresWithRoadAccess.Add(new Vector3S(entry.Key));
            }
        }

        groups = SquareGroup.GetGroupsFromMap(this.squares).OrderByDescending(group => group.ConnectedCount).ToList();
    }

    public void SpawnAtRandom(SpawnableBuilding spawnableBuilding) {
        Building building = Building.TryPlaceOnMap(this, spawnableBuilding);
        if (building != null) {
            AddBuildingToMap(building);
        }
    }

    public void FillGroup(int ID, List<BuildingSpawnItem> items) {
        List<Vector3S> toDo = groups[ID].SquaresWithRoadAccess;
        System.Random random = new System.Random();
        toDo = toDo.OrderBy(v => random.Next()).ToList();
        
        while (toDo.Count > 0) {
            Vector3S current = toDo[0];
            Building building = null;
            foreach (BuildingSpawnItem item in items) {
                if (CheckBuildingAmount(item.building.buildingTypeID, item.amount)) {
                    building = Building.FitsBuilding(squares[current], item.building, this);
                    if (building != null) {
                        break;
                    }
                }
            }

            if (building == null) break; 
            else {
                AddBuildingToMap(building);
                foreach (Vector3S v in building.squares) if (toDo.Contains(v)) toDo.Remove(v);
            }
        }
    }

    public void AddBuildingToMap(Building building) {
        AddToCount(building);
        foreach (Vector3S v in building.squares) {takenSquares.Add(v);}
        buildings.Add(building);
    }
}
