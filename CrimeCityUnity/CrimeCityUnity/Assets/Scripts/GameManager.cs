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

    void Start() {
        mapRenderer = GetComponent<MapRenderer>();
        trafficRenderer = GetComponent<TrafficRenderer>();
        if (GetMapOnLoad) GetMap(); traffic = new TrafficManager(map);

        TrafficEntity trafficEntity = new TrafficEntity(map.Streets[0].NodeIDs[0], 0);
        traffic.AddEntity(trafficEntity, trafficRenderer);
        trafficEntity.SetPath(map.Streets[0]);

    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            GetMap();
        }
    }

    void FixedUpdate() {
        if (traffic == null) traffic = new TrafficManager(map);
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
    }
}
