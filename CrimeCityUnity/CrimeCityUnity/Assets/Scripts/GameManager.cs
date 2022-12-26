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
    public TrafficManager traffic;

    void Start() {
        if (GetMapOnLoad) GetMap();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            GetMap();
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

        MapRenderer renderer = GetComponent<MapRenderer>();

        renderer.ClearLineObjects();
        if (renderer.renderNodes) renderer.DrawNodes(map.Nodes);
        renderer.DrawPaths(map, 0f);

        traffic = new TrafficManager(map);

        //temp 
        GetComponent<TrafficRenderer>().ClearTrafficEntities();
        for (int i = 0; i < map.Streets.Count; i++) {
            TrafficEntity trafficEntity = new TrafficEntity(map.Streets[i].NodeIDs[0], i);
            trafficEntity.SetPath(map.Streets[i]);
            traffic.AddEntity(trafficEntity);
        }
        GetComponent<TrafficRenderer>().SpawnTrafficEntities(traffic.Entities, map);
    }
}
