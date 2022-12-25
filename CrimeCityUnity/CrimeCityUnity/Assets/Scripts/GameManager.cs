using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LSystemGenerator systemGenerator;
    public bool LoadFromSave = false;

    public StreetMap map;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            
            if (LoadFromSave) {
                map = SaveLoad.GetSave().GetMap();
            } else {
                string sentence = systemGenerator.GenerateSentence();
                map = Decoder.GetMap(sentence, Vector3.zero, systemGenerator);
                SaveLoad.Save(map);
            }
            GetComponent<LinesRenderer>().DrawNodes(map.Nodes, 0.5f);

            foreach (StreetPath path in map.Streets) {
                GetComponent<LinesRenderer>().DrawPath(path, map.Nodes, Color.red, 3f, 5f);
            }
        }
    }
}
