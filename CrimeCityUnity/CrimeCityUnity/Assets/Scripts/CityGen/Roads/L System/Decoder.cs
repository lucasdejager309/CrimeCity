using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;


public static class Decoder {
    [System.Serializable]
    public class ActionKey {
        public char character;
        public Decoder.Action action;

        public ActionKey(char c, Decoder.Action action) {
            this.character = c;
            this.action = action;
        }
    }

    [System.Serializable]
    public enum Action {
        save,
        load,
        draw,
        turnRight,
        turnLeft,
        none
    }

    public static Action charToAction(List<ActionKey> keys, char c) {
        foreach (var a in keys) {
            if (a.character == c) {
                return a.action;
            }
        } 

        return Action.none;
    }

    public static StreetMap GetMap(string sentence, Vector3 startPosition, LSystemGenerator systemGenerator) {
        List<StreetSection> streetSections = new List<StreetSection>();

        Stack<AgentParameters> savePoints = new Stack<AgentParameters>();
        List<StreetNode> nodes = new List<StreetNode>();

        Vector3 currentPos = startPosition;
        nodes.Add(new StreetNode(currentPos));

        Vector3 direction = Vector3.forward;
        Vector3 tempPosition = startPosition;

        float length = systemGenerator.startLength;

        //decode sentence
        foreach (char c in sentence) {
            Action action = charToAction(systemGenerator.keys, c);

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
                    tempPosition = currentPos;
                    currentPos += (direction*length); 
                    //round to 2 decimals to prevent duplicate nodes with like a 0.000001 difference
                    currentPos = new Vector3((float)Math.Round(currentPos.x, 2), (float)Math.Round(currentPos.y, 2), (float)Math.Round(currentPos.z, 2));
                    
                    if (systemGenerator.useBound) {
                        if ((!inBounds(currentPos, startPosition, systemGenerator.outerBound, systemGenerator.boundType))) {
                            currentPos -= (direction*length);
                            break;
                        }  
                    }

                    StreetSection sectionToAdd = new StreetSection(tempPosition, currentPos);
                    if (!StreetNode.ContainsPosition(nodes, currentPos)) {
                        nodes.Add(sectionToAdd.endNode);
                    }
                    
                    if (!StreetSection.ContainsPath(streetSections, sectionToAdd)) {
                        streetSections.Add(sectionToAdd);
                    }

                    length *= systemGenerator.lengthModifier;
                    
                    break;
                case Action.turnRight:
                    direction = Quaternion.AngleAxis(systemGenerator.GetAngle(), Vector3.up) * direction;
                    break;
                case Action.turnLeft:
                    direction = Quaternion.AngleAxis(-systemGenerator.GetAngle(), Vector3.up) * direction;
                    break;
                case Action.none:
                    break;
                default:
                    break;
            }
        }

        //remove duplicates
        streetSections = StreetSection.Distinct(streetSections);
        nodes = StreetNode.Distinct(nodes);

        //add connection to nodes
        for (int i = 0; i < nodes.Count; i++) {
            foreach (StreetSection section in streetSections) {
                if (section.ContainsNodePosition(nodes[i].position)) {
                    nodes[i].AddConnection(section.GetOtherNode(nodes[i]).position, nodes);
                }
            }
        }

        return new StreetMap(streetSections, nodes);
    }

    [System.Serializable]
    public enum BoundType {
        ROUND,
        SQUARE
    }

    private static bool inBounds(Vector3 pos, Vector3 center, float bound, BoundType boundType = BoundType.SQUARE) {
        switch (boundType) {
            case (BoundType.SQUARE):
                if (pos.x >= center.x+bound || pos.x <= center.x-bound) return false;
                if (pos.z >= center.z+bound || pos.z <= center.z-bound) return false;
                return true;
            case (BoundType.ROUND):
                if (Vector3.Distance(center, pos) >= bound) return false;
                return true;
            default:
                break;
        }
        return false;
    }

}
