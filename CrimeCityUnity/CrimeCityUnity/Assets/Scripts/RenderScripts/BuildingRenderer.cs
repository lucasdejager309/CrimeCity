using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingRenderer : MonoBehaviour
{
    List<GameObject> buildingObjects = new List<GameObject>();

    public void DrawBuildings(List<Building> buildings, List<SpawnableBuilding> spawnableBuildings, float scale = 1) {
        foreach (Building building in buildings) {  
            foreach (SpawnableBuilding sB in spawnableBuildings) {
                if (sB.buildingTypeID == building.buildingTypeID) {
                    GameObject obj = Instantiate(sB.prefab, building.bound.GetCenter(), Quaternion.identity);
                    obj.transform.localScale = new Vector3(obj.transform.localScale.x*scale, obj.transform.localScale.y*scale, obj.transform.localScale.z*scale);
                    buildingObjects.Add(obj);
                    break;
                }
            }
        }
    }

    public void ClearBuildingObjects() {
        foreach(GameObject obj in buildingObjects) {
            Destroy(obj);
        }

        buildingObjects.Clear();
    }
}
