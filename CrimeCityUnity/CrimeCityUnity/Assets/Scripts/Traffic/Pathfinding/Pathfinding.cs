using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Pathfinding {

    public static StreetPath FindPath(int start, int end, StreetMap map) {
		PathNode startNode = new PathNode(start);
        PathNode endNode = new PathNode(end);
        
        List<PathNode>openList = new List<PathNode>() { startNode };
		List<PathNode>closedList = new List<PathNode>() {};

		startNode.gCost = 0;
		startNode.hCost = PathNode.CalculateDistance(startNode, endNode, map.Nodes);

		while (openList.Count > 0) {
			PathNode currentNode = PathNode.GetLowestFcostNode(openList);

			if (map.Nodes[currentNode.streetNodeID].Position == map.Nodes[endNode.streetNodeID].Position) {
				return CalculatePath(startNode, currentNode, map.Nodes);
			}

			openList.Remove(currentNode);
			closedList.Add(currentNode);


			foreach(PathNode neighbourNode in currentNode.GetNeighbouringNodes(map.Nodes)) {
				if (PathNode.ContainsID(closedList, neighbourNode.streetNodeID)) continue;

				float tentativeGCost = currentNode.gCost + PathNode.CalculateDistance(currentNode, neighbourNode, map.Nodes);
				if (tentativeGCost < neighbourNode.gCost) {
					neighbourNode.cameFromNode = currentNode;

					neighbourNode.gCost = tentativeGCost;
					neighbourNode.hCost = PathNode.CalculateDistance(neighbourNode, endNode, map.Nodes);


					if (!openList.Contains(neighbourNode)) {
						openList.Add(neighbourNode);
					}
				}
			}
		}

		return null;
	}

	private static StreetPath CalculatePath(PathNode startNode, PathNode endNode, List<Node> mapNodes) {
		StreetPath path = new StreetPath(new List<int>(), mapNodes);

		path.NodeIDs.Add(endNode.streetNodeID);
		PathNode currentNode = endNode;
		while (currentNode.cameFromNode != null) {
			path.NodeIDs.Add(currentNode.streetNodeID);
			currentNode = currentNode.cameFromNode;
		}
		path.NodeIDs.Add(startNode.streetNodeID);

		path.NodeIDs.Reverse();
		path.SetLength(path.CalculateLength(mapNodes));
		return path;
	}
}