using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


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
                    currentPos += direction*length;
                    if (!StreetSection.ContainsPath(streetSections, new StreetSection(tempPosition, currentPos))) {
                        streetSections.Add(new StreetSection(tempPosition, currentPos));
                        length *= systemGenerator.lengthModifier;
                    }
                    if (!StreetNode.ContainsPosition(nodes, currentPos)) nodes.Add(new StreetNode(currentPos));
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

        streetSections = StreetSection.Distinct(streetSections);

        nodes = StreetNode.Distinct(nodes);

        return new StreetMap(streetSections, nodes);
    }
}
