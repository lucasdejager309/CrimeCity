using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[System.Serializable]
public enum BoundType {
    ROUND,
    SQUARE
}


[System.Serializable]
public class ActionKey {
    public char character;
    public Action action;
    public int times = 1;
    public bool randomTimes = false;
    [Range(0, 10)]
    public int minTimes = 1;
    [Range(0, 10)]
    public int maxTimes = 4;

    public ActionKey(char c, Action action, int times = 1)
    {
        this.character = c;
        this.action = action;
        this.times = times;
    }
}

[System.Serializable]
public enum Action {
    save,
    load,
    draw,
    turnRight,
    turnLeft,
    none
}


[CreateAssetMenu(fileName = "New LSystemGenerator", menuName = "LSystemGenerator")]
public class LSystemGenerator : ScriptableObject
{
    [Header("Generation Settings")]
    public string rootSentence = null;
    [Range(0, 20)]
    public int iterationLimit = 1;
    public float startLength;
    public bool generateStreets = false;
    
    [Header("Restrictions")]
    public bool useBound = false;
    public BoundType boundType = BoundType.SQUARE;
    public float outerBound;
    
    [Header("Angle")]
    private float angle = 90;
    private bool useRandomAngle = false;
    [Range(0, 180)]
    private float minAngle;
    [Range(0, 180)]
    private float maxAngle;

    [Header("Generation Logic")]
    public Rule[] rules;
    [SerializeField]
    public List<ActionKey> keys = new List<ActionKey>();

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

    public static ActionKey charToAction(List<ActionKey> keys, char c) {
        foreach (var a in keys) {
            if (a.character == c) {
                return a;
            }
        } 

        return null;
    }

    public static bool InBounds(Vector3 pos, Vector3 center, float bound, BoundType boundType = BoundType.SQUARE) {
        switch (boundType) {
            case (BoundType.SQUARE):
                if (pos.x >= center.x+bound || pos.x <= center.x-bound) return false;
                if (pos.z >= center.z+bound || pos.z <= center.z-bound) return false;
                return true;
            case (BoundType.ROUND):
                if (Vector3.Distance(center, pos) >= bound) return false;
                return true;
            default:
                break;
        }
        return false;
    }
}
