using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LSystemGenerator systemGenerator;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            string sentence = systemGenerator.GenerateSentence();
            List<StreetSection> streetSections = Decoder.GetLines(sentence, Vector3.zero, systemGenerator);
            Debug.Log("streetSections:" + streetSections.Count + "\nSentence: " + sentence);

            GetComponent<LinesRenderer>().DrawLines(streetSections);
        }
    }
}
