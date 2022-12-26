using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrafficManager
{
    [SerializeField] List<TrafficEntity> entities = new List<TrafficEntity>();
    public List<TrafficEntity> Entities {
        get {return entities;}
    }
    [SerializeField] StreetMap map;

    public TrafficManager(StreetMap map) {
        this.map = map;
    }

    public void AddEntity(TrafficEntity entity) {
        entities.Add(entity);
    }

    public void SetPath(int entityID, StreetPath path) {
        entities[entityID].SetPath(path);
    }

    public void Update() {
        foreach (TrafficEntity e in entities) {
            e.Update(map);
        }
    }
}
