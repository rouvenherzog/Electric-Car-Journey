using System.Collections.Generic;
using TurnTheGameOn.SimpleTrafficSystem;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public static TargetManager Instance { get; private set; }

    private void Awake() 
    { 
        if (Instance != null && Instance != this) 
            Destroy(this); 
        else 
            Instance = this; 
    }

    public int TargetCount;
    public TargetPosition TargetPositionPrefab;
    public List<TargetPosition> Targets = new();

    public void Start() {
        WaypointManager waypointManager = WaypointManager.Instance;
        if (waypointManager.Initalized) SpawnTargets();
        else waypointManager.OnInitialized.AddListener(SpawnTargets);
    }

    private void SpawnTargets() {
        for(int i = 0; i  < TargetCount; i++) {
            AITrafficWaypoint waypoint =
                WaypointManager.Instance.GetRandomVisitableWaypoint();
            TargetPosition target =
                Instantiate(TargetPositionPrefab, transform);
            target.Waypoint = waypoint;
            Targets.Add(target);
        }
    }
}
