using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Square {
    private Vector3S centerPos = null;

    public Vector3S[] points {get; private set;} = new Vector3S[4];
    //              point  nodeID  
    public Dictionary<int, int> streetNodes {get; private set;} = new Dictionary<int, int>();

    public List<Vector3S> connectedSquares = new List<Vector3S>();
    
    public List<int> streetZDirections = new List<int>();
    public List<int> streetXDirections = new List<int>();

    public Vector3 position {
        get {
            return points[0].Back();
        }
    }

    public Square(Vector3 newPosition, float gridSize) {
        
        points[0] = new Vector3S(newPosition);
        points[1] = new Vector3S(newPosition + new Vector3(gridSize, 0, 0));
        points[2] = new Vector3S(newPosition + new Vector3(gridSize, 0,  gridSize));
        points[3] = new Vector3S(newPosition + new Vector3(0, 0, gridSize));
    }

    public Vector3 GetCenterPos() {
        if (centerPos != null) return centerPos.Back();
        float x = 0f;
        float y = 0f;
        float z = 0f;
        foreach (Vector3S point in points)
        {
            x += point.x;
            y += point.y;
            z += point.z;
        }
        centerPos = new Vector3S(new Vector3(x / points.Length, y / points.Length, z / points.Length));
        return centerPos.Back();
    }

    public Dictionary<int, int> GetNodesOnGrid(List<Node> nodes) {    
        int? possibleNode = null;
        for (int i = 0; i < points.Length; i++) {
            possibleNode = Node.GetIDofPos(points[i].Back(), nodes);
            if (possibleNode != null) {
                streetNodes.Add(i, (int)possibleNode);
                break;
            }
        }

        if (possibleNode == null) return null; 
        else {
            foreach (int c in nodes[(int)possibleNode].GetConnectedNodes()) {
                for (int i = 0; i < points.Length; i++) {
                    possibleNode = Node.GetIDofPos(points[i].Back(), nodes);
                    if (possibleNode != null) {
                        if (!streetNodes.ContainsKey(i)) streetNodes.Add(i, (int)possibleNode);
                    }
                }
            }
        }

        return streetNodes;
    }

    public int GetzLayerDirection(List<Node> nodes) {
        int direction = 0;
        
        if (
            ( // 0, 1
                streetNodes.ContainsKey(0) && streetNodes.ContainsKey(1)
                &&
                nodes[streetNodes[0]].GetConnectedNodes().Contains(streetNodes[1])
            )
        ) {
            direction = 1;
            streetZDirections.Add(-1);
        } 
        if (
            ( // 2, 3
                streetNodes.ContainsKey(2) && streetNodes.ContainsKey(3)
                &&
                nodes[streetNodes[2]].GetConnectedNodes().Contains(streetNodes[3])
            )
        ) {
            direction = -1;
            streetZDirections.Add(1);
        }

        return direction;
    }

    public int GetxLayerDirection(List<Node> nodes) {
        int direction = 0;

        if (
            ( // 0, 3
                streetNodes.ContainsKey(0) && streetNodes.ContainsKey(3)
                &&
                nodes[streetNodes[0]].GetConnectedNodes().Contains(streetNodes[3])
            )
        ) {
            direction = 1;
            streetXDirections.Add(-1);
        }
        if (
            ( // 1, 2
                streetNodes.ContainsKey(1) && streetNodes.ContainsKey(2)
                &&
                nodes[streetNodes[1]].GetConnectedNodes().Contains(streetNodes[2])
            )
        ) {

            direction = -1;
            streetXDirections.Add(1);
        }

        return direction;
    }

    public List<Vector3S> FitsBuilding(SpawnableBuilding building, BuildingMap map) {
        List<Vector3S> takenSquares = new List<Vector3S>();

        for (int x = 0; x < building.xSize; x++) {
            for (int z = 0; z < building.zSize; z++) {
                Vector3S currentPos = new Vector3S((x*map.gridSize)+position.x, 0, (z*map.gridSize)+position.z);

                if (takenSquares.Count == 0) {
                    //first square
                    takenSquares.Add(currentPos);
                } else {

                    bool posIsConnected = false;
                    foreach(Vector3S takenSquare in takenSquares) {
                        if (map.Squares[takenSquare].connectedSquares.Contains(currentPos)) {
                            posIsConnected = true;
                        }
                    }

                    if (!posIsConnected) return null;
                    else {
                        if (!takenSquares.Contains(currentPos)) takenSquares.Add(currentPos);
                        //check if road is in building place...
                    }
                }

            }
        }
        
        if (takenSquares.Count < building.xSize*building.zSize) return null;
        return takenSquares;
    }

    public static Dictionary <Vector3, Square> GetConnectedSquares(Dictionary <Vector3, Square> squares, float gridSize) {
        foreach (var kv in squares) {
            //x
            if (!kv.Value.streetXDirections.Contains(-1)) {
                if (squares.ContainsKey(kv.Key + new Vector3(-gridSize, 0, 0))) {
                    kv.Value.connectedSquares.Add(new Vector3S(kv.Key));
                }
            }
            if (!kv.Value.streetXDirections.Contains(1)) {
                if (squares.ContainsKey(kv.Key + new Vector3(gridSize, 0, 0))) {
                    kv.Value.connectedSquares.Add(new Vector3S(kv.Key + new Vector3(-gridSize, 0, 0)));
                }
            }

            //z
            if (!kv.Value.streetZDirections.Contains(-1)) {
                if (squares.ContainsKey(kv.Key + new Vector3(0, 0, -gridSize))) {
                    kv.Value.connectedSquares.Add(new Vector3S(kv.Key + new Vector3(0, 0, -gridSize)));
                }
            }

            if (!kv.Value.streetZDirections.Contains(1)) {
                if (squares.ContainsKey(kv.Key + new Vector3(0, 0, gridSize))) {
                    kv.Value.connectedSquares.Add(new Vector3S(kv.Key + new Vector3(0, 0, gridSize)));
                }
            }

            //+x+z
            if (!kv.Value.streetXDirections.Contains(1) && !kv.Value.streetZDirections.Contains(1)) {
                if (squares.ContainsKey(kv.Key + new Vector3(gridSize, 0, gridSize))) {
                    kv.Value.connectedSquares.Add(new Vector3S(kv.Key + new Vector3(gridSize, 0, gridSize)));
                }
            }

            //+x-z
            if (!kv.Value.streetXDirections.Contains(1) && !kv.Value.streetZDirections.Contains(-1)) {
                if (squares.ContainsKey(kv.Key + new Vector3(gridSize, 0, -gridSize))) {
                    kv.Value.connectedSquares.Add(new Vector3S(kv.Key + new Vector3(gridSize, 0, -gridSize)));
                }
            }

            //-x+z
            if (!kv.Value.streetXDirections.Contains(-1) && !kv.Value.streetZDirections.Contains(1)) {
                if (squares.ContainsKey(kv.Key + new Vector3(-gridSize, 0, gridSize))) {
                    kv.Value.connectedSquares.Add(new Vector3S(kv.Key + new Vector3(-gridSize, 0, gridSize)));
                }
            }

            //-x-z
            if (!kv.Value.streetXDirections.Contains(-1) && !kv.Value.streetZDirections.Contains(-1)) {
                if (squares.ContainsKey(kv.Key + new Vector3(-gridSize, 0, -gridSize))) {
                    kv.Value.connectedSquares.Add(new Vector3S(kv.Key + new Vector3(-gridSize, 0, -gridSize)));
                }
            }
            // Debug.Log(kv.Key + " has " + kv.Value.connectedSquares.Count + " connected squares");
        }
    
        return squares;
    }
}