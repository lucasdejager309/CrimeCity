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

    public static List<KeyValuePair<Vector3, Vector3>> GetLines(string sentence, Vector3 startPosition, LSystemGenerator systemGenerator) {
        List<KeyValuePair<Vector3, Vector3>> lines = new List<KeyValuePair<Vector3, Vector3>>();

        Stack<AgentParameters> savePoints = new Stack<AgentParameters>();
        List<Vector3> nodePositions = new List<Vector3>();

        Vector3 currentPos = startPosition;
        if (!nodePositions.Contains(startPosition)) nodePositions.Add(startPosition);

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
                    lines.Add(new KeyValuePair<Vector3, Vector3>(tempPosition, currentPos));
                    length *= systemGenerator.lengthModifier;
                    nodePositions.Add(currentPos);
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

        lines = RemoveDuplicateLines(lines);

        return lines;
    }

    private static List<KeyValuePair<Vector3, Vector3>> RemoveDuplicateLines(List<KeyValuePair<Vector3, Vector3>> lines) {
        List<KeyValuePair<Vector3, Vector3>> linesToRemove = new List<KeyValuePair<Vector3, Vector3>>();
        
        foreach(var line in lines) {
            foreach(var lineToCompare in lines) {
                if (
                    (
                        (line.Key == lineToCompare.Key && line.Value == lineToCompare.Value) 
                        ||
                        (line.Key == lineToCompare.Value && line.Value == lineToCompare.Key)
                    )
                    &&
                    (!line.Equals(lineToCompare) && !linesToRemove.Contains(lineToCompare))
                ) {
                    linesToRemove.Add(new KeyValuePair<Vector3, Vector3>(line.Key, line.Value));
                }
            }
        }


        //Debug.Log("culled " + linesToRemove.Count + "/" + lines.Count + " lines"); this is not accurate (i'm sure it's nothing to worry about)
        return (lines.Except(linesToRemove)).ToList();
    }
}
