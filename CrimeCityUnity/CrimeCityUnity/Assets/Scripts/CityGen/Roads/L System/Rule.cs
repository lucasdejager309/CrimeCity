using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Rule {
    public string character;
    [SerializeField] private bool randomResult;
    [SerializeField] private string[] results;

    public string GetResult() {
        if (!randomResult) {
            return results[0];
        }
        else {
            return results[(int)Random.Range(0, results.Length)];
        }
    }
}
