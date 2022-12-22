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
        Stack<AgentParameters> savePoints = new Stack<AgentParameters>();
        
        StreetMap map = new StreetMap();
        
        Vector3 currentPos = startPosition;
        map.nodes.Add(new StreetNode(currentPos, map.nodes.Count));

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
                            currentPos = tempPosition;
                            break;
                        }  
                    }

                    if (!StreetNode.ContainsPosition(map.nodes, currentPos)) {
                        map.nodes.Add(new StreetNode(currentPos, map.nodes.Count));
                    }

                    StreetSection sectionToAdd = new StreetSection(tempPosition, currentPos, 0, map);
                    if (!StreetSection.ContainsPath(map.sections, sectionToAdd, map)) {
                        map.sections.Add(new StreetSection(sectionToAdd.startID, sectionToAdd.endID, map.sections.Count));
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

        //add connection to nodes
        for (int i = 0; i < map.nodes.Count; i++) {
            foreach (StreetSection section in map.sections) {
                if (section.ContainsNodePosition(map.nodes[i].position, map)) {
                    map.nodes[i].AddConnection(section.GetOtherPosition(map.nodes[i].position, map), map.nodes);
                }
            }
        }

        return map;
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
