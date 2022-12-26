using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficRenderer : MonoBehaviour
{
    public GameObject tempCarPrefab;
    public Dictionary<int, GameObject> trafficDict = new Dictionary<int, GameObject>();
    
    public void SpawnTrafficEntities(List<TrafficEntity> entities, StreetMap map) {
        foreach (TrafficEntity entity in entities) {
            SpawnTrafficEntity(entity, map);
        }
    }
    
    public void ClearTrafficEntities() {
        foreach (var kv in trafficDict) {
            Destroy(kv.Value);
        }

        trafficDict.Clear();
    }

    public void SpawnTrafficEntity(TrafficEntity entity, StreetMap map) {
        Vector3 position = map.Nodes[entity.currentNode].Position;

        GameObject trafficObject = Instantiate(tempCarPrefab, position, Quaternion.identity);
        trafficDict.Add(entity.ID, trafficObject);
    }

    public void UpdateTraffic(List<TrafficEntity> entities, StreetMap map) {
        foreach (var kv in trafficDict) {
            Vector3? position = map.Nodes[entities[kv.Key].currentNode].Position;
            if (entities[kv.Key].nextNode != null) {
                position = map.Nodes[entities[kv.Key].currentNode].Position + (map.Nodes[(int)entities[kv.Key].nextNode].Position - map.Nodes[entities[kv.Key].currentNode].Position).normalized * entities[kv.Key].progress;
            }
            kv.Value.transform.position = (Vector3)position;
        }
    }
}
