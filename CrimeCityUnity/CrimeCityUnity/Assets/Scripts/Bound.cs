using UnityEngine;

[System.Serializable]
public class Bound {
        public float xMin = float.MaxValue;
        public float xMax = -float.MaxValue;
        public float zMin  = float.MaxValue;
        public float zMax = -float.MaxValue;

        public void Extend(float amount) {
            xMin -= amount;
            zMin -= amount;
            xMax += amount;
            zMax += amount;
        }

        public void Update(Vector3 position) {
            if (position.x < xMin) {xMin = position.x;}
            if (position.z < zMin) {zMin = position.z;}
            if (position.x > xMax) {xMax = position.x;}
            if (position.z > zMax) {zMax = position.z;}
        }

        public Vector3 GetCenter() {
            return new Vector3((xMax+xMin)/2, 0, (zMax+zMin)/2);
        }
}