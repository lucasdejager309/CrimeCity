using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[CreateAssetMenu(fileName = "New RoadTypeDetector", menuName = "RoadTypeDetector")]
[System.Serializable]
public class RoadTypeDetector : ScriptableObject
{
    public TextAsset[] prefixLists;
    public TextAsset roadNames;
    public List<RoadType> types;

    public string GetName(Road road) {
        string name = null;
        Dictionary<string, string[]> roadTypeNames = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(roadNames.text);
        
        foreach (RoadType type in types) {
            if (road.length > type.minLength) {
                string[] possibleNames = roadTypeNames[type.roadNameTypes[Random.Range(0, type.roadNameTypes.Length)]];
                name = possibleNames[Random.Range(0, possibleNames.Length)];
            }
        }

        List<string> animals = JsonConvert.DeserializeObject<List<string>>(prefixLists[Random.Range(0, prefixLists.Length)].text);

        return animals[Random.Range(0, animals.Count-1)] + " " + name;
    }

    public float GetSpeedLimit(Road road) {
        float? speedLimit = null;
        foreach (RoadType type in types) {
            if (road.length > type.minLength) {
                speedLimit = road.speedLimit*type.speedModifier;
            }
        }

        return (float)speedLimit;
    }
}

[System.Serializable]
public class RoadType {
    public float minLength;
    public string[] roadNameTypes;
    public float speedModifier = 1f;
}
