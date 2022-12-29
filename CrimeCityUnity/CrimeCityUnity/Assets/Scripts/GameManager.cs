using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LSystemGenerator systemGenerator;
    public bool LoadFromSave = false;
    public bool GetMapOnLoad = false;

    public StreetMap map;
    MapRenderer mapRenderer;
    public TrafficManager traffic;
    TrafficRenderer trafficRenderer;

    //temp testing
    TrafficEntity trafficEntity;
    GameObject pathObject;

    void Start() {
        mapRenderer = GetComponent<MapRenderer>();
        trafficRenderer = GetComponent<TrafficRenderer>();
        if (GetMapOnLoad) GetMap();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            GetMap();
        }
        
        if (Input.GetKeyDown(KeyCode.LeftControl)) {
            Destroy(pathObject);
            StreetPath path = Pathfinding.FindPath(trafficEntity.currentNode, map.Nodes[Random.Range(0, map.Nodes.Count-1)].ID, map);
            pathObject = mapRenderer.DrawPath(path, map.Nodes, Color.red, 1f, 1f);
            trafficEntity.SetPath(path);
        }
    }

    void FixedUpdate() {
        traffic.Update();
        GetComponent<TrafficRenderer>().UpdateTraffic(traffic.Entities, map);
    }

    void GetMap() {
        if (LoadFromSave) {
            map = SaveLoad.GetSave().GetMap();
        } else {
            string sentence = systemGenerator.GenerateSentence();
            map = Decoder.GetMap(sentence, Vector3.zero, systemGenerator);
            SaveLoad.Save(map);
        }
        mapRenderer.ClearLineObjects();
        if (mapRenderer.renderNodes) mapRenderer.DrawNodes(map.Nodes);
        mapRenderer.DrawPaths(map, 0f);
        
        traffic = new TrafficManager(map);
        traffic.ClearTraffic();
        trafficRenderer.ClearTrafficEntities();


        //temp testing
        int startNode = map.Nodes[Random.Range(0, map.Nodes.Count-1)].ID;
        int endNode = map.Nodes[Random.Range(0, map.Nodes.Count-1)].ID;

        trafficEntity = new TrafficEntity(startNode, 0);
        traffic.AddEntity(trafficEntity, trafficRenderer);
    }
}
