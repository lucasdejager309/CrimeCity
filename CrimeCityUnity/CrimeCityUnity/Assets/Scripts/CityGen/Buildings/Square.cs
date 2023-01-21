using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Square {
    private Vector3S centerPos = null;

    public Vector3S[] points {get; private set;} = new Vector3S[4];
    //              point  nodeID  
    public Dictionary<int, int> streetNodes {get; private set;} = null;

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
        if (streetNodes == null) {
            streetNodes = new Dictionary<int, int>();
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

    public static Dictionary <Vector3, Square> GetConnectedSquares(Dictionary <Vector3, Square> squares, float gridSize) {
        foreach (var kv in squares) {
            //x
            if (!kv.Value.streetXDirections.Contains(-1)) {
                if (squares.ContainsKey(kv.Key + new Vector3(-gridSize, 0, 0))) {
                    kv.Value.connectedSquares.Add(new Vector3S(kv.Key + new Vector3(-gridSize, 0, 0)));
                }
            }
            if (!kv.Value.streetXDirections.Contains(1)) {
                if (squares.ContainsKey(kv.Key + new Vector3(gridSize, 0, 0))) {
                    kv.Value.connectedSquares.Add(new Vector3S(kv.Key + new Vector3(gridSize, 0, 0)));
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
            if (!kv.Value.streetXDirections.Contains(1) && !kv.Value.streetZDirections.Contains(1) && !kv.Value.streetXDirections.Contains(1)) {
                if (squares.ContainsKey(kv.Key + new Vector3(gridSize, 0, gridSize))) {
                    kv.Value.connectedSquares.Add(new Vector3S(kv.Key + new Vector3(gridSize, 0, gridSize)));
                }
            }

            //+x-z
            if (!kv.Value.streetXDirections.Contains(1) && !kv.Value.streetZDirections.Contains(-1) && !kv.Value.streetXDirections.Contains(1)) {
                if (squares.ContainsKey(kv.Key + new Vector3(gridSize, 0, -gridSize))) {
                    kv.Value.connectedSquares.Add(new Vector3S(kv.Key + new Vector3(gridSize, 0, -gridSize)));
                }
            }

            //-x+z
            if (!kv.Value.streetXDirections.Contains(-1) && !kv.Value.streetZDirections.Contains(1) && !kv.Value.streetXDirections.Contains(-1)) {
                if (squares.ContainsKey(kv.Key + new Vector3(-gridSize, 0, gridSize))) {
                    kv.Value.connectedSquares.Add(new Vector3S(kv.Key + new Vector3(-gridSize, 0, gridSize)));
                }
            }

            //-x-z
            if (!kv.Value.streetXDirections.Contains(-1) && !kv.Value.streetZDirections.Contains(-1) && !kv.Value.streetXDirections.Contains(-1)) {
                if (squares.ContainsKey(kv.Key + new Vector3(-gridSize, 0, -gridSize))) {
                    kv.Value.connectedSquares.Add(new Vector3S(kv.Key + new Vector3(-gridSize, 0, -gridSize)));
                }
            }
        }
    
        return squares;
    }
}

[System.Serializable]
public class SquareGroup {
    public int ID;
    
    [SerializeField] List<Vector3S> squares = new List<Vector3S>();
    public List<Vector3S> Squares {
        get {return squares; }
    }

    [SerializeField] List<Vector3S> squaresWithRoadAccess = new List<Vector3S>();
    public List<Vector3S> SquaresWithRoadAccess {
        get {return squaresWithRoadAccess; }
    }

    //amount of squares in group
    public int Count {
        get {return squares.Count; }
    }
    //amount of squares with road acces in group
    public int ConnectedCount {
        get {return squaresWithRoadAccess.Count; }
    }

    //ratio of squares with road access to total squares
    public float RoadAccesRatio {
        get {return (float)(((float)ConnectedCount/(float)Count)); }
    }

    //temp
    // Vector3S averagePosition = null;
    // public Vector3S AveragePosition() {
    //     if (averagePosition == null) averagePosition = Vector3S.Average(squares);
    //     return averagePosition;
    // }

    public SquareGroup(List<Vector3S> positions, Dictionary<Vector3S, Square> squares, int ID) {
        foreach (Vector3S v in positions) {
            this.squares.Add(v);
            if (squares[v].streetNodes.Count > 0) this.squaresWithRoadAccess.Add(v);
        }
        this.ID = ID;
    }

    public static SquareGroup GetFromConnected(Vector3S start, Dictionary<Vector3S, Square> squares, int ID) {
        List<Vector3S> groupedSquares = new List<Vector3S>();
        groupedSquares.Add(start);

        List<Vector3S> toCheck = new List<Vector3S>();
        toCheck.Add(start);

        while(toCheck.Count > 0) {
            Vector3S current = toCheck[0];

            foreach (Vector3S v in squares[current].connectedSquares) {
                if (!groupedSquares.Contains(v)) {
                    groupedSquares.Add(v);
                    if (!toCheck.Contains(v)) toCheck.Add(v);
                }
            }

            toCheck.Remove(current);
        }

        return new SquareGroup(groupedSquares, squares, ID);
    }

    public static List<SquareGroup> GetGroupsFromMap(Dictionary<Vector3S, Square> squares) {
        List<SquareGroup> groups = new List<SquareGroup>();
        
        List<Vector3S> toDo = squares.Keys.ToList();

        while (toDo.Count > 0) {
            SquareGroup newGroup = GetFromConnected(toDo[0], squares, groups.Count);
            foreach (Vector3S v in newGroup.squares) toDo.Remove(v);
            groups.Add(newGroup);
        }

        return groups;
    }
}