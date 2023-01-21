using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Vector3S {
    public float x {get; private set;}
    public float y {get; private set;}
    public float z {get; private set;}

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

    public override string ToString() {
        return "(" + x + ", " + y + ", " + z + ")";
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Vector3S vector = obj as Vector3S;
        if ((vector.x == x) && (vector.y == y) && (vector.z == z)) {
            return true;
        } else return false;
    }

    public override int GetHashCode()
    {
        int hash = 5303;
        hash = hash*3733+x.GetHashCode();
        hash = hash*3733+y.GetHashCode();
        hash = hash*3733+z.GetHashCode();

        return hash;
    }

    public static Vector3S Average(List<Vector3S> vectors) {
        Vector3 average = new Vector3();

        for (int i = 0; i < vectors.Count; i++) {
            average += vectors[i].Back();
        }

        return new Vector3S(average.x/vectors.Count, average.y/vectors.Count, average.z/vectors.Count);
    }
}
