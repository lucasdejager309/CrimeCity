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

    public Building(string buildingTypeID, Vector3 position, int ID) {
        this.ID = ID;
        this.buildingTypeID = buildingTypeID;
        this.position = new Vector3S(position);
    }

    public static List<Vector3S> GetPossiblePosition(BuildingMap map, SpawnableBuilding building, Vector3? position = null, float maxDistanceFromPos = 0) {
        if (position == null) {
            foreach (Vector3S positionS in map.squaresWithRoadAccess) {
                
                List<Vector3S> squares = map.Squares[positionS].FitsBuilding(building, map);
                if (squares != null) {
                    return squares;
                }
            }
        } else {
            //magic
        }

        return null;
    } 
}
