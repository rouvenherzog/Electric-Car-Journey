using System.Collections;
using System.Collections.Generic;
using TurnTheGameOn.SimpleTrafficSystem;
using UnityEditor.PackageManager;
using UnityEngine;
using System.Linq;

public static class PathFinder
{
    public class WaypointNode {
        public int Cost = 0;
        public float Distance;
        public AITrafficWaypoint Waypoint;
        public WaypointNode ParentNode;

        public WaypointNode(WaypointNode parent, AITrafficWaypoint node, AITrafficWaypoint target) {
            ParentNode = parent;
            Waypoint = node;
            Cost = parent != null ? parent.Cost + 1 : 0;
            Distance = Vector3.Distance(target.transform.position, node.transform.position);
        }
    }

    public static List<AITrafficWaypoint> ShortestPath(AITrafficWaypoint start, AITrafficWaypoint end) {
        WaypointNode startNode = new(null, start, end);
        Dictionary<AITrafficWaypoint, bool> visited = new();

        List<WaypointNode> possibleNodes = new();
        foreach(AITrafficWaypoint node in GetNextNodes(start)) {
            possibleNodes.Add(new WaypointNode(startNode, node, end));
        }

        while(possibleNodes.Count > 0) {
            WaypointNode currentNode = possibleNodes.OrderBy(node => node.Distance).First();
            if (currentNode.Waypoint == end) {
                return BuildPath(currentNode);
            }

            visited.Add(currentNode.Waypoint, true);
            possibleNodes.Remove(currentNode);
            List<AITrafficWaypoint> nextNodes =
                GetNextNodes(currentNode.Waypoint);
            foreach(AITrafficWaypoint node in nextNodes) {
                if (visited.ContainsKey(node)) continue;

                WaypointNode existingNode = possibleNodes.Find(possibleNode => possibleNode.Waypoint == node);
                WaypointNode newNode = new WaypointNode(currentNode, node, end);
                if (existingNode != null) {
                    if (existingNode.Cost > newNode.Cost) {
                        possibleNodes.Remove(existingNode);
                        possibleNodes.Add(newNode);
                    }
                } else {
                    possibleNodes.Add(newNode);
                }
            }
        }

        Debug.Log($"No path found between {start} and {end}");
        return null;
    }

    private static List<AITrafficWaypoint> BuildPath(WaypointNode finalNode) {
        List<AITrafficWaypoint> result = new();
        WaypointNode currentNode = finalNode;
        while(currentNode != null) {
            result.Insert(0, currentNode.Waypoint);
            currentNode = currentNode.ParentNode;
        }
        return result;
    }

    public static List<AITrafficWaypoint> GetNextNodes(AITrafficWaypoint node) {
        AITrafficWaypointSettings nodeSettings = node.onReachWaypointSettings;
        int nodeIndex = nodeSettings.waypointIndexnumber - 1;
        AITrafficWaypointRoute parentRoute = nodeSettings.parentRoute;
        
        List<AITrafficWaypoint> result = new(nodeSettings.newRoutePoints);
        if (nodeIndex < (parentRoute.waypointDataList.Count - 1))
            result.Add(parentRoute.waypointDataList[nodeIndex + 1]._waypoint);

        return result;
    }
}
