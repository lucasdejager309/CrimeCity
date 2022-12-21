using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LSystemGenerator systemGenerator;
    public bool LoadFromSave = false;

    public StreetMap map;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            
            
            if (LoadFromSave) {
                map = StreetMap.LoadMap(SaveLoad.GetSave());
            } else {
                string sentence = systemGenerator.GenerateSentence();
                map = Decoder.GetMap(sentence, Vector3.zero, systemGenerator);
                SaveLoad.Save(map);
            }
            GetComponent<LinesRenderer>().DrawLines(map);
        }
    }
}
