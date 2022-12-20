using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinesRenderer : MonoBehaviour
{
    public Material lineMaterial;
    public Color color;
    public float width;

    public List<GameObject> lineObjects = new List<GameObject>();

    public void DrawLines(List<StreetSection> sections) {
        ClearLineObjects();
        
        foreach (StreetSection section in sections) {
            GameObject line = new GameObject("line");
            lineObjects.Add(line);
            line.transform.position = section.startNode.position;
            var lineRenderer = line.AddComponent<LineRenderer>();
            lineRenderer.material = lineMaterial;
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
            lineRenderer.startWidth = width;
            lineRenderer.endWidth = width;
            lineRenderer.SetPosition(0, section.startNode.position);
            lineRenderer.SetPosition(1, section.endNode.position);
        }
    }

    private void ClearLineObjects() {
        foreach(GameObject obj in lineObjects) {
            Destroy(obj);
        }

        lineObjects.Clear();
    }
}
