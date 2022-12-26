using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrafficEntity {
    public int ID {get; private set;}

    public int currentNode;
    public int? nextNode;
    public float progress = 0;
    public bool canMove = true;

    public Task moveTask {get; private set;}
    
    private StreetPath pathToFollow;
    public StreetPath PathToFollow {
        get {return pathToFollow;}
    }
    private int pathIndex;
    public void SetPath(StreetPath newPath) {
        pathToFollow = newPath;
        pathIndex = 0;
        currentNode = pathToFollow.NodeIDs[0];
    }

    public TrafficEntity(int node, int id) {
        ID = id;
        currentNode = node;
    }

    public void Update(StreetMap map) {
        if (pathToFollow != null) {
            if (nextNode == null && pathIndex < pathToFollow.NodeIDs.Count) {
                nextNode = pathToFollow.NodeIDs[pathIndex+1];
            }

            if (canMove) {  
                if (moveTask == null) {
                    progress = 0; 
                    moveTask = new Task(MoveBetween(0.04f, map.Nodes[currentNode], map.Nodes[(int)nextNode]));
                    moveTask.Finished += delegate {
                        
                        pathIndex++;
                        if (pathIndex+1 < pathToFollow.NodeIDs.Count) {
                            //continue
                            moveTask = null;
                            currentNode = pathToFollow.NodeIDs[pathIndex];
                            nextNode = pathToFollow.NodeIDs[pathIndex+1];
                        } else {
                            //end
                            moveTask = null;
                            currentNode = pathToFollow.NodeIDs[pathIndex];
                            nextNode = null;
                            pathToFollow = null;     
                        }
                    };
                }
            } 
        }
    }
    
    public IEnumerator MoveBetween(float speed, Node startNode, Node endNode) {
        float end = Node.Distance(startNode, endNode);

        float elapsedTime = 0;
        progress = 0;

        while (elapsedTime < end/speed) {
            progress += speed;
            if (progress > end) {
                progress = end;
                break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
