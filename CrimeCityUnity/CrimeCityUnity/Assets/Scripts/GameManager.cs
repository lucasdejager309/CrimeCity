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

    void Start() {
        mapRenderer = GetComponent<MapRenderer>();
        trafficRenderer = GetComponent<TrafficRenderer>();
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
        GetComponent<TrafficRenderer>().UpdateTraffic(traffic.Entities, cityGen.map);
    }

    void InitMap() {
        cityGen.GetMap(LoadFromSave);
        mapRenderer.ClearLineObjects();
        if (mapRenderer.renderNodes) mapRenderer.DrawNodes(cityGen.map.Nodes);
        mapRenderer.DrawStreets(cityGen.map, 0f);
        mapRenderer.ClearSquareObjects();
        mapRenderer.DrawSquares(cityGen.map.Squares);

        traffic = new TrafficManager(cityGen.map);
        traffic.ClearTraffic();
        trafficRenderer.ClearTrafficEntities();
    }
}
