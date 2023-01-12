using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingRenderer : MonoBehaviour
{
    public bool drawEdgeNodes;
    public bool drawBuildings;
    [SerializeField] List<GameObject> buildingObjects = new List<GameObject>();

    public void DrawBuildings(BuildingMap map, List<SpawnableBuilding> spawnableBuildings, float scale = 1) {
        foreach (Building building in map.buildings) {  
            if (drawBuildings) {
                foreach (SpawnableBuilding sB in spawnableBuildings) {
                    if (sB.buildingTypeID == building.buildingTypeID) {
                        GameObject obj = Instantiate(sB.prefab, building.bound.GetCenter(), Quaternion.identity);
                        obj.transform.localScale = new Vector3(obj.transform.localScale.x*scale, obj.transform.localScale.y*scale, obj.transform.localScale.z*scale);
                        
                        if (building.direction == 'W') {
                            obj.transform.RotateAround(obj.transform.position, obj.transform.up, -90);
                        } else if (building.direction == 'E') {
                            obj.transform.RotateAround(obj.transform.position, obj.transform.up, 90);
                        } else if (building.direction == 'N') {
                            obj.transform.RotateAround(obj.transform.position, obj.transform.up, 180);
                        }
                        
                        buildingObjects.Add(obj);
                        break;
                    }
                }
            }
            

            if (drawEdgeNodes) {
                foreach (var node in building.edgeNodes) {
                    GetComponent<MapRenderer>().DrawNode(node.Key.Back(), Color.red);
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
