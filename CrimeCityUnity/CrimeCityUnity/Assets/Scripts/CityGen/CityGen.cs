using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityGen : MonoBehaviour
{
    [SerializeField] private LSystemGenerator systemGenerator;
    [SerializeField] private RoadTypeDetector roadTypeDetector;
    public BuildingGen buildingGen;

    public float minimumSquares;

    public StreetMap streetMap = new StreetMap();
    public BuildingMap buildingMap = new BuildingMap();

    public void GetMap() {
        Clear();
        while (buildingMap.Squares.Count < minimumSquares) {
            string sentence = systemGenerator.GenerateSentence();
            streetMap = Decoder.GetMap(sentence, Vector3.zero, systemGenerator, roadTypeDetector);
            buildingMap = buildingGen.GetMap(streetMap.Nodes, systemGenerator.startLength);
        }

        streetMap.AddBuildingConnections(buildingMap.buildings);
        SaveLoad.SaveCity(streetMap, buildingMap);
    }

    public void LoadMap() {
        CitySave save = SaveLoad.GetCitySave(); 
        streetMap = save.GetStreetMap();
        buildingMap = save.GetBuildingMap();
    }

    private void Clear() {
        streetMap = new StreetMap();
        buildingMap = new BuildingMap();
    }
}
