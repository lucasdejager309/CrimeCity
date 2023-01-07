using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class CitySave {
    StreetMap streetMap;
    BuildingMap buildingMap;
    
    public StreetMap GetStreetMap() {
        return streetMap;
    }
    public BuildingMap GetBuildingMap() {
        return buildingMap;
    }
    
    public CitySave(StreetMap streetMap, BuildingMap buildingMap) {
        if (streetMap != null) this.streetMap = streetMap;
        if (streetMap != null) this.buildingMap = buildingMap;
    }
}

public static class SaveLoad {

    public static void SaveCity(StreetMap streetMap = null, BuildingMap buildingMap = null) {
        CitySave save = new CitySave(streetMap, buildingMap);

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, save);
        stream.Close();
    }

    public static CitySave GetCitySave() {
        string path = Application.persistentDataPath + "/save.data";

        if(File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            CitySave save = formatter.Deserialize(stream) as CitySave;
            stream.Close();

            return save; 
        } else Debug.Log("No Save Found!"); return null;
    }
}