using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;



[CreateAssetMenu(menuName ="CityGen/SystemGenerator")]
public class LSystemGenerator : ScriptableObject
{
    public Rule[] rules;
    public string rootSentence = null;
    [Range(0, 20)]
    public int iterationLimit = 1;
    public float startLength;
    public float lengthModifier;
    [SerializeField] private float angle;
    public bool useRandomAngle;
    [Range(0, 180)]
    [SerializeField] private float minAngle;
    [Range(0, 180)]
    [SerializeField] private float maxAngle;

    [SerializeField]
    public List<Decoder.ActionKey> keys = new List<Decoder.ActionKey>();

    public string GenerateSentence(string sentence = null) {
        if (sentence == null) {
            sentence = rootSentence;
        }

        return GrowRecursive(sentence);
    }

    private string GrowRecursive(string sentence, int index = 0) {
        if (index >= iterationLimit) {
            return sentence;
        }

        StringBuilder newSentence = new StringBuilder();
        foreach (char c in sentence) {
            newSentence.Append(c);
            ProcesRulesRecursive(newSentence, c, index);
        }

        return newSentence.ToString();
    }

    private void ProcesRulesRecursive(StringBuilder newSentence, char c, int index) {
        foreach (Rule r in rules) {
            if (r.character == c.ToString()) {
                newSentence.Append(GrowRecursive(r.GetResult(), index + 1));
            }
        }
    }

    public float GetAngle() {
        float angletoReturn;
        if (!useRandomAngle) angletoReturn = angle;
        else angletoReturn = Random.Range(minAngle, maxAngle+1);

        return angletoReturn;
    }
}
