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

    //temp testing
    TrafficEntity trafficEntity;
    GameObject pathObject;

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
        
        //temp testing
        if (Input.GetKeyDown(KeyCode.LeftControl)) {
            Destroy(pathObject);
            StreetPath path = Pathfinding.FindPath(trafficEntity.currentNode, cityGen.map.Nodes[Random.Range(0, cityGen.map.Nodes.Count-1)].ID, cityGen.map);
            pathObject = mapRenderer.DrawPath(path, cityGen.map.Nodes, Color.red, 1f, 1f);
            trafficEntity.SetPath(path);
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

        traffic = new TrafficManager(cityGen.map);
        traffic.ClearTraffic();
        trafficRenderer.ClearTrafficEntities();


        //temp testing
        int startNode = cityGen.map.Nodes[Random.Range(0, cityGen.map.Nodes.Count-1)].ID;
        int endNode = cityGen.map.Nodes[Random.Range(0, cityGen.map.Nodes.Count-1)].ID;

        trafficEntity = new TrafficEntity(startNode, 0);
        traffic.AddEntity(trafficEntity, trafficRenderer);
    }
}
