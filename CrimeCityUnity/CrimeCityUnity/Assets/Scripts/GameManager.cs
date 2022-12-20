using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LSystemGenerator systemGenerator;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            string sentence = systemGenerator.GenerateSentence();
            List<KeyValuePair<Vector3, Vector3>> lines = Decoder.GetLines(sentence, Vector3.zero, systemGenerator);
            Debug.Log("Sentence: " + sentence + "\nlinecount:" + lines.Count);

            GetComponent<LinesRenderer>().DrawLines(lines);
        }
    }
}
