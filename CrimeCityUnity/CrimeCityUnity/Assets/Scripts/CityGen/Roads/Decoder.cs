using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public static class Decoder 
{
    public static StreetMap GetMap(string sentence, Vector3 startPos, LSystemGenerator generator, RoadTypeDetector roadTypeDetector) {
        
        Stack<AgentParameters> savePoints = new Stack<AgentParameters>();

        List<Node> nodes = new List<Node>();

        Vector3 currentPos = startPos;
        nodes.Add(new Node(currentPos, nodes.Count));

        Vector3 direction = Vector3.forward;
        Vector3 lastPos = startPos;

        float length = generator.startLength;

        foreach (char c in sentence) {
            ActionKey action = LSystemGenerator.charToAction(generator.keys, c);

            int times = action.times;
            if (action.randomTimes) {
                times = UnityEngine.Random.Range(action.minTimes, action.maxTimes);
            }

            for (int i = 0; i < times; i++) {
                switch (action.action) {
                    case Action.save:
                        savePoints.Push(new AgentParameters {
                            position = currentPos,
                            direction = direction,
                            length = length 
                        });
                        break;
                    case Action.load:
                        if (savePoints.Count > 0) {
                            AgentParameters agentParameters = savePoints.Pop();
                            currentPos = agentParameters.position;
                            direction = agentParameters.direction;
                            length = agentParameters.length;
                        } else {Debug.Log("No savePoints to load!");}
                        break;
                    case Action.draw:
                        lastPos = currentPos;
                        currentPos += (direction*length);
                        //round to 2 decimals to prevent duplicate nodes with like a 0.000001 difference
                        currentPos = new Vector3((float)Math.Round(currentPos.x, 2), (float)Math.Round(currentPos.y, 2), (float)Math.Round(currentPos.z, 2));

                        if (generator.useBound) {
                            if ((!LSystemGenerator.InBounds(currentPos, startPos, generator.outerBound, generator.boundType))) {
                                i = times;
                                currentPos = lastPos;
                                break;
                            }
                        }

                        nodes = Draw(currentPos, lastPos, nodes);

                        length *= generator.lengthModifier;

                        break;
                    case Action.turnRight:
                        direction = Quaternion.AngleAxis(generator.GetAngle(), Vector3.up) * direction;
                        break;
                    case Action.turnLeft:
                        direction = Quaternion.AngleAxis(-generator.GetAngle(), Vector3.up) * direction;
                        break;
                }
            }
            
        }

        StreetMap map = new StreetMap(nodes, null);  

        foreach (Road street in map.Streets) {
            street.SetName(roadTypeDetector.GetName(street));
            street.speedLimit = roadTypeDetector.GetSpeedLimit(street);
        }

        return map;
    }

    private static List<Node> Draw(Vector3 pos1, Vector3 pos2, List<Node> nodes) {
        int node1ID;
        int node2ID;
        int? tempID;

        tempID = Node.GetIDofPos(pos1, nodes);
        if (tempID == null) {
            //if pos1 is not yet in nodes
            //add new node
            node1ID = nodes.Count;
            nodes.Add(new Node(pos1, node1ID));
        } else {
            //else get nodeID
            node1ID = (int)tempID;
        }
        tempID = Node.GetIDofPos(pos2, nodes);
        if (tempID == null) {
            //if pos2 is not yet in nodes
            //add new node
            node2ID = nodes.Count;
            nodes.Add(new Node(pos2, node2ID));
        } else {
            //else get nodeID
            node2ID = (int)tempID;
        }

        nodes[node1ID].AddConnectedNode(node2ID);
        nodes[node2ID].AddConnectedNode(node1ID);

        return nodes;
    }
}
