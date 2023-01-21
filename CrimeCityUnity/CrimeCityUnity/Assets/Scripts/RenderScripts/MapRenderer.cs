using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRenderer : MonoBehaviour
{
    public float roadHeight = 0.5f;
    public Material lineMaterial;
    public Color color;
    public float width;
    public bool renderNodes = false;
    public bool renderStreets = false;
    public GameObject nodePrefab;
    

    public List<GameObject> lineObjects = new List<GameObject>();

    public void DrawNodes(List<Node> nodes, float ypos = 0) {
        foreach (Node n in nodes) {
            DrawNode(n.Position);
        }
    }

    public void DrawNode(Vector3 position, Color? color = null) {
        if (color == null) {
            color = Color.red;
        }

        GameObject nodeObject = Instantiate(nodePrefab, position, Quaternion.identity);
        nodeObject.GetComponent<MeshRenderer>().material.color = (Color)color;
        lineObjects.Add(nodeObject);
    }

    public void DrawStreets(StreetMap map, Color? color = null, float? width = null) {
        
        if (color == null) color = this.color;
        if (width == null) width = this.width;

        foreach(Road street in map.Streets) {
            
            GameObject obj = DrawPath(street, map.Nodes, (Color)color, (float)width, roadHeight);
            obj.name = street.name;
        }
    }

    public GameObject DrawPath(StreetPath path, List<Node> nodes, Color color, float width, float yPos = 1) {
        List<Vector3> positions = new List<Vector3>();
        
        if (path.NodeIDs.Count > 1) {
            foreach (int id in path.NodeIDs) {
                positions.Add(nodes[id].Position);
            }
            return CreateLineObject(positions, color, width, yPos);
        }   
        return null;
    }

    public GameObject CreateLineObject(List<Vector3> positions, Color color, float width, float yPos = 1) {
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

    public void ClearLineObjects() {
        foreach(GameObject obj in lineObjects) {
            Destroy(obj);
        }

        lineObjects.Clear();
    }
}
