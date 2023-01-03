using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityGen : MonoBehaviour
{
    [SerializeField] private LSystemGenerator systemGenerator;
    [SerializeField] private RoadTypeDetector roadTypeDetector;
    [SerializeField] private BuildingGen buildingGen;
    public StreetMap map;

    public void GetMap(bool loadFromSave) {
        if (loadFromSave) {
            map = SaveLoad.GetSave().GetMap();
        } else {
            string sentence = systemGenerator.GenerateSentence();
            map = Decoder.GetMap(sentence, Vector3.zero, systemGenerator, roadTypeDetector);
            map.SetSquares(buildingGen.GetSquares(map.Nodes, systemGenerator.startLength));

            SaveLoad.Save(map);
        }
    }
}
