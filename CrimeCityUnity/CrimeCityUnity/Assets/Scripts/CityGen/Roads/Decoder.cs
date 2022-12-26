using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Decoder 
{
    public static StreetMap GetMap(string sentence, Vector3 startPos, LSystemGenerator generator) {
        
        Stack<AgentParameters> savePoints = new Stack<AgentParameters>();

        List<Node> nodes = new List<Node>();
        List<NodeConnection> connections = new List<NodeConnection>();
        int nodeconnectionID = 0;

        Vector3 currentPos = startPos;
        nodes.Add(new Node(currentPos, nodes.Count));

        Vector3 direction = Vector3.forward;
        Vector3 tempPos = startPos;

        float length = generator.startLength;

        foreach (char c in sentence) {
            Action action = LSystemGenerator.charToAction(generator.keys, c);

            switch (action) {
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
                    tempPos = currentPos;
                    currentPos += (direction*length);
                    //round to 2 decimals to prevent duplicate nodes with like a 0.000001 difference
                    currentPos = new Vector3((float)Math.Round(currentPos.x, 2), (float)Math.Round(currentPos.y, 2), (float)Math.Round(currentPos.z, 2));

                    if (generator.useBound) {
                        if ((!LSystemGenerator.InBounds(currentPos, startPos, generator.outerBound, generator.boundType))) {
                            currentPos = tempPos;
                            break;
                        }
                    }

                    int node1ID;
                    int node2ID;
                    int? tempID;

                    tempID = Node.GetIDofPos(currentPos, nodes);
                    if (tempID == null) {
                        node1ID = nodes.Count;
                        nodes.Add(new Node(currentPos, node1ID));
                    } else {
                        node1ID = (int)tempID;
                    }
                    tempID = Node.GetIDofPos(tempPos, nodes);
                    if (tempID == null) {
                        node2ID = nodes.Count;
                        nodes.Add(new Node(tempPos, node2ID));
                    } else {
                        node2ID = (int)tempID;
                    }

                    nodes[node1ID].AddConnectedNode(node2ID);
                    nodes[node2ID].AddConnectedNode(node1ID);

                    NodeConnection connectionToAdd = new NodeConnection(nodeconnectionID, node1ID, node2ID, nodes);
                    nodeconnectionID++;
                    if (!(connections.Contains(connectionToAdd)) || !(NodeConnection.GetConnectionBetween(connections, node1ID, node2ID) == null)) {
                        connections.Add(connectionToAdd);
                    }

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
        
        return new StreetMap(nodes, connections);
    }
}
