using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinesRenderer : MonoBehaviour
{
    public Material lineMaterial;
    public Color color;
    public float width;

    public List<GameObject> lineObjects = new List<GameObject>();

    public void DrawLines(List<KeyValuePair<Vector3, Vector3>> lines) {
        ClearLineObjects();
        
        foreach (var kv in lines) {
            GameObject line = new GameObject("line");
            lineObjects.Add(line);
            line.transform.position = kv.Key;
            var lineRenderer = line.AddComponent<LineRenderer>();
            lineRenderer.material = lineMaterial;
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
            lineRenderer.startWidth = width;
            lineRenderer.endWidth = width;
            lineRenderer.SetPosition(0, kv.Key);
            lineRenderer.SetPosition(1, kv.Value);
        }
    }

    private void ClearLineObjects() {
        foreach(GameObject obj in lineObjects) {
            Destroy(obj);
        }

        lineObjects.Clear();
    }
}
