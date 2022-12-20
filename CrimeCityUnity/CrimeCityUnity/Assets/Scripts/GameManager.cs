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
            Debug.Log("streetSections: " + map.sections.Count + "\nnodes: " + map.nodes.Count + "\nSentence: " + sentence);

            GetComponent<LinesRenderer>().DrawLines(map);
            
        }
    }
}
