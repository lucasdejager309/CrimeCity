using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class Save {
    StreetMap map;
    
    public StreetMap GetMap() {
        return map;
    }
    
    public Save(StreetMap map) {
        this.map = map;
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

        public Vector3S(float x, float y, float z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3 Back() {
            return new Vector3(x, y, z);
        }

        public static Vector3 Back(Vector3S vector) {
            return new Vector3(vector.x, vector.y, vector.z);
        }
    }
