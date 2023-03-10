using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New SpawnableBuilding", menuName = "SpawnableBuilding")]
public class SpawnableBuilding : ScriptableObject
{
    public string buildingTypeID;
    public int xSize;
    public int zSize;
    public int size {
        get {
            return xSize*zSize;
        }
    }

    public GameObject[] prefabs;
    public GameObject randomPrefab {
        get {
            return prefabs[Random.Range(0, prefabs.Length)];
        }
    }
}

