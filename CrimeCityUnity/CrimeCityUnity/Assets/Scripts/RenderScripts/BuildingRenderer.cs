using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingRenderer : MonoBehaviour
{
    public bool renderEdgeNodes;
    public bool renderBuildings;
    public bool renderSquares = false;
    public GameObject squarePrefab;

    [Range(0, 1)]
    public float scaleFactor = 1f;
    [SerializeField] List<GameObject> buildingObjects = new List<GameObject>();
    [SerializeField] List<GameObject> squareObjects = new List<GameObject>();

    public void DrawBuildings(BuildingMap map, List<BuildingSpawnItem> spawnableBuildings, float scale = 1) {
        foreach (Building building in map.buildings) {  
            if (renderBuildings) {
                foreach (BuildingSpawnItem item in spawnableBuildings) {
                    if (item.building.buildingTypeID == building.buildingTypeID) {
                        GameObject obj = Instantiate(item.building.randomPrefab, building.bound.GetCenter(), Quaternion.identity);
                        obj.name = building.buildingTypeID;
                        obj.transform.localScale = new Vector3(obj.transform.localScale.x*map.gridSize*scaleFactor, obj.transform.localScale.y*map.gridSize*scaleFactor, obj.transform.localScale.z*map.gridSize*scaleFactor);
                        
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
            

            if (renderEdgeNodes) {
                foreach (var node in building.edgeNodes) {
                    GetComponent<MapRenderer>().DrawNode(node.Key.Back(), Color.red);
                }
            }
        }
    }

    public void DrawSquares(Dictionary<Vector3S, Square> squares) {
        foreach (var square in squares) {
            CreateSquareObject(square.Value, square.Key.ToString());
        }
    }

    public void CreateSquareObject(Square square, string name) {
        Vector3 position = square.GetCenterPos();
        GameObject squareObject = Instantiate(squarePrefab, position, Quaternion.identity);
        squareObject.name = name;

        float gridSize = square.points[1].x-square.points[0].x;
        squareObject.transform.localScale = new Vector3(gridSize*0.7f, 1, gridSize*0.7f);
        squareObjects.Add(squareObject);
    }

    public void ClearBuildingObjects() {
        foreach(GameObject obj in buildingObjects) {
            Destroy(obj);
        }

        buildingObjects.Clear();
    }

    public void ClearSquareObjects() {
        foreach(GameObject obj in squareObjects) {
            Destroy(obj);
        }

        squareObjects.Clear();
    }
}
