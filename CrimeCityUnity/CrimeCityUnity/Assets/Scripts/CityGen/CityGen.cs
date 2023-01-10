using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityGen : MonoBehaviour
{
    [SerializeField] private LSystemGenerator systemGenerator;
    [SerializeField] private RoadTypeDetector roadTypeDetector;
    public BuildingGen buildingGen;
    public StreetMap streetMap;
    public BuildingMap buildingMap;

    public void GetMap(bool loadFromSave) {
        if (loadFromSave) {
            CitySave save = SaveLoad.GetCitySave(); 
            streetMap = save.GetStreetMap();
            buildingMap = save.GetBuildingMap();
        } else {
            string sentence = systemGenerator.GenerateSentence();
            streetMap = Decoder.GetMap(sentence, Vector3.zero, systemGenerator, roadTypeDetector);
            buildingMap = buildingGen.GetMap(streetMap.Nodes, systemGenerator.startLength);
            streetMap.AddBuildingConnections(buildingMap.buildings);

            SaveLoad.SaveCity(streetMap, buildingMap);
        }
    }
}
