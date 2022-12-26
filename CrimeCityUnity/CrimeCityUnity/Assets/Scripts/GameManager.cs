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

            MapRenderer renderer = GetComponent<MapRenderer>();

            renderer.ClearLineObjects();
            if (renderer.renderNodes) renderer.DrawNodes(map.Nodes);
            renderer.DrawPaths(map, 0f);
        }
    }
}
