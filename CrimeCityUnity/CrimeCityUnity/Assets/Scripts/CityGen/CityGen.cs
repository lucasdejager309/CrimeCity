using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityGen : MonoBehaviour
{
    [SerializeField] private LSystemGenerator systemGenerator;
    [SerializeField] private RoadTypeDetector roadTypeDetector;
    [SerializeField] private int buildingLayers = 1; //temp
    public StreetMap map;

    public void GetMap(bool loadFromSave) {
        if (loadFromSave) {
            map = SaveLoad.GetSave().GetMap();
        } else {
            string sentence = systemGenerator.GenerateSentence();
            map = Decoder.GetMap(sentence, Vector3.zero, systemGenerator, roadTypeDetector);
            map.SetSquares(BuildingGen.GetSquares(map.Nodes, systemGenerator.startLength, buildingLayers));

            SaveLoad.Save(map);
        }
    }
}
