using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LSystemGenerator systemGenerator;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            string sentence = systemGenerator.GenerateSentence();
            StreetMap map = Decoder.GetMap(sentence, Vector3.zero, systemGenerator);
            
            GetComponent<LinesRenderer>().DrawLines(map);
        }
    }
}
