using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinesRenderer : MonoBehaviour
{
    public Material lineMaterial;
    public Color color;
    public float width;
    public GameObject nodePrefab;

    public List<GameObject> lineObjects = new List<GameObject>();

    public void DrawLines(StreetMap map) {
        ClearLineObjects();
        
        foreach (StreetSection section in map.sections) {
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

        foreach (StreetNode node in map.nodes) {
            GameObject nodeObject = Instantiate(nodePrefab, node.position, Quaternion.identity);
            lineObjects.Add(nodeObject);
        }
    }

    private void ClearLineObjects() {
        foreach(GameObject obj in lineObjects) {
            Destroy(obj);
        }

        lineObjects.Clear();
    }
}
