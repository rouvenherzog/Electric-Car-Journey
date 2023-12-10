using System.Collections.Generic;
using TurnTheGameOn.SimpleTrafficSystem;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using UnityEditor.Experimental.GraphView;

public class WaypointManager : MonoBehaviour
{
    public bool Initalized = false;
    public UnityEvent OnInitialized = new();
    public static WaypointManager Instance { get; private set; }
    public List<AITrafficWaypoint> Waypoints = new();
    public List<AITrafficWaypoint> VisitableWaypoints = new();

    private void Awake() 
    { 
        if (Instance != null && Instance != this) 
            Destroy(this); 
        else 
            Instance = this; 
    }

    private void Start() {
        AITrafficWaypointRoute[] routes =
            FindObjectsOfType<AITrafficWaypointRoute>();
        foreach(AITrafficWaypointRoute route in routes) {
            List<AITrafficWaypoint> nodes =
                route.waypointDataList.Select(node => node._waypoint).ToList();
            Waypoints.AddRange(nodes);
            if (!nodes.Any(IsVisitable)) continue;
            VisitableWaypoints.AddRange(nodes.Where(IsVisitable));
        }
        Initalized = true;
        OnInitialized.Invoke();
    }

    private bool IsVisitable(AITrafficWaypoint waypoint) {
        int visitableLayer = LayerMask.NameToLayer("Visitable");
        return waypoint.gameObject.layer == visitableLayer;
    }

    public AITrafficWaypoint GetClosestWaypoint(Vector3 position) {
        return Waypoints
            .OrderBy(waypoint =>
                Vector3.Distance(waypoint.transform.position, position)
            )
            .First();
    }

    public AITrafficWaypoint GetRandomVisitableWaypoint() {
        return VisitableWaypoints[Random.Range(0, VisitableWaypoints.Count)];
    }

    public Quaternion GetWaypointRotation(AITrafficWaypoint waypoint) {
        List<AITrafficWaypoint> list = new();
        int waypointIndex =
            waypoint.onReachWaypointSettings.waypointIndexnumber;
        List<CarAIWaypointInfo> parentRoute =
            waypoint.onReachWaypointSettings.parentRoute.waypointDataList;
        if (waypointIndex > 1)
            list.Add(parentRoute[waypointIndex - 2]._waypoint);
        list.Add(waypoint);
        if (waypointIndex < parentRoute.Count)
            list.Add(parentRoute[waypointIndex]._waypoint);
        
        Vector3 direction =
            (list.Last().transform.position - list.First().transform.position)
                .normalized;
        return Quaternion.LookRotation(direction, Vector3.up);
    }
}
