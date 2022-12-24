using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class Save {
    List<KeyValuePair<Vector3S, List<int>>> saveNodes = new List<KeyValuePair<Vector3S, List<int>>>();
    
    public StreetMap GetMap() {
        StreetMap map = new StreetMap();
        foreach (var node in saveNodes) {
            map.Nodes.Add(new StreetNode(Vector3S.ConvertBack(node.Key), map.Nodes.Count, node.Value));
        }

        return map;
    }
    
    public Save(StreetMap map) {
        foreach (StreetNode node in map.Nodes) {
            saveNodes.Add(new KeyValuePair<Vector3S, List<int>>(new Vector3S(node.Position), node.GetConnectedNodes()));
        }
    }
}

public static class SaveLoad {

    public static void Save(StreetMap streetMap) {
        Save save = new Save(streetMap);

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, save);
        stream.Close();
    }

    public static Save GetSave() {
        string path = Application.persistentDataPath + "/save.data";

        if(File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            Save save = formatter.Deserialize(stream) as Save;
            stream.Close();

            return save; 
        } else Debug.Log("No Save Found!"); return null;
    }
}

[System.Serializable]
public class Vector3S {
        public float x;
        public float y;
        public float z;

        public Vector3S(Vector3 vector3) {
            x = vector3.x;
            y = vector3.y;
            z = vector3.z;
        }

        public static Vector3 ConvertBack(Vector3S vector) {
            return new Vector3(vector.x, vector.y, vector.z);
        }
    }
