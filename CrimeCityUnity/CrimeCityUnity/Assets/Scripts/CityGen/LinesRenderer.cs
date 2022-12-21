using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinesRenderer : MonoBehaviour
{
    public Material lineMaterial;
    public Color color;
    public float width;
    public bool renderNodes = false;
    public GameObject nodePrefab;

    public List<GameObject> lineObjects = new List<GameObject>();

    public void DrawLines(StreetMap map) {
        ClearLineObjects();
        
        foreach (StreetSection section in map.sections) {
            List<Vector3> positions = new List<Vector3>();
            positions.Add(section.StartNode(map).position);
            positions.Add(section.EndNode(map).position);

            lineObjects.Add(CreateLineObject(positions, color, width));
        }

        if (renderNodes) {
            foreach (StreetNode node in map.nodes) {
                GameObject nodeObject = Instantiate(nodePrefab, node.position, Quaternion.identity);
                lineObjects.Add(nodeObject);
            }
        }

        
    }

    public GameObject CreateLineObject(List<Vector3> positions, Color color, float width, float yPos = 0) {
        GameObject line = new GameObject("line");
        lineObjects.Add(line);
        line.transform.position = positions[0];
        line.transform.rotation = Quaternion.Euler(90, 0, 0);

        var lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.alignment = LineAlignment.TransformZ;
        lineRenderer.material = lineMaterial;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.positionCount = positions.Count;
        
        for (int i = 0; i < positions.Count; i++) {
            Vector3 positionToDraw = positions[i];
            if (i == 0) {
                positionToDraw -= (positions[i+1]-positions[i]).normalized*(width/2);
            }
            else if (i == positions.Count-1) {
                positionToDraw -= (positions[i-1]-positions[i]).normalized*(width/2);
            }
            positionToDraw = new Vector3(positionToDraw.x, yPos, positionToDraw.z);

            lineRenderer.SetPosition(i, positionToDraw);
        }

        return line;
    }

    private void ClearLineObjects() {
        foreach(GameObject obj in lineObjects) {
            Destroy(obj);
        }

        lineObjects.Clear();
    }
}
