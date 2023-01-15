using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    
    public bool LoadFromSave = false;
    public bool GetMapOnLoad = false;

    CityGen cityGen;
    MapRenderer mapRenderer;
    public TrafficManager traffic;
    TrafficRenderer trafficRenderer;
    BuildingRenderer buildingRenderer;

    void Start() {
        mapRenderer = GetComponent<MapRenderer>();
        trafficRenderer = GetComponent<TrafficRenderer>();
        buildingRenderer = GetComponent<BuildingRenderer>();
        cityGen = GetComponent<CityGen>();
        if (GetMapOnLoad) {
            InitMap();    
        }
        
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            InitMap();
        }
    }

    void FixedUpdate() {
        traffic.Update();
        GetComponent<TrafficRenderer>().UpdateTraffic(traffic.Entities, cityGen.streetMap);
    }

    void InitMap() {
        cityGen.GetMap(LoadFromSave);
        
        //draw streets
        mapRenderer.ClearLineObjects();
        if (mapRenderer.renderNodes) mapRenderer.DrawNodes(cityGen.streetMap.Nodes);
        mapRenderer.DrawStreets(cityGen.streetMap);
        
        //draw squares
        mapRenderer.ClearSquareObjects();
        if (mapRenderer.renderSquares) mapRenderer.DrawSquares(cityGen.buildingMap.Squares);

        //draw buildings
        buildingRenderer.ClearBuildingObjects();
        buildingRenderer.DrawBuildings(cityGen.buildingMap, cityGen.buildingGen.buildings, cityGen.buildingMap.gridSize);

        traffic = new TrafficManager(cityGen.streetMap);
        traffic.ClearTraffic();
        trafficRenderer.ClearTrafficEntities();
    }
}
