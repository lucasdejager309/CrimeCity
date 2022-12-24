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

    public void DrawLines(List<StreetNode> nodes, float yPos = 0) {
        ClearLineObjects();
        
        List<KeyValuePair<Vector3, Vector3>> drawnLines = new List<KeyValuePair<Vector3, Vector3>>();

        foreach (StreetNode node in nodes) {
            foreach (int connectedNode in node.GetConnectedNodes()) {
                if (!ContainsLine(node.Position, nodes[connectedNode].Position, drawnLines)) {
                    List<Vector3> positions = new List<Vector3>();
                    positions.Add(node.Position);
                    positions.Add(nodes[connectedNode].Position);

                    CreateLineObject(positions, color, width, yPos);
                    drawnLines.Add(new KeyValuePair<Vector3, Vector3>(node.Position, nodes[connectedNode].Position));
                }
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

    private bool ContainsLine(Vector3 pos1, Vector3 pos2, List<KeyValuePair<Vector3, Vector3>> lines) {
        foreach (var line in lines) {
            if (
                (line.Key == pos1 && line.Value == pos2)
                ||
                (line.Key == pos2 && line.Value == pos1)
            ) return true;
        }
        return false;
    }
}
