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
    public int EnergyCount;
    public TargetPosition TargetPositionPrefab;
    public TargetPosition EnergyPrefab;
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

        for(int i = 0; i  < EnergyCount; i++) {
            AITrafficWaypoint waypoint =
                WaypointManager.Instance.GetRandomVisitableWaypoint();
            TargetPosition target =
                Instantiate(EnergyPrefab, transform);
            target.Waypoint = waypoint;
        }
    }

    public void Regenerate() {
        Targets.Clear();
        
        foreach(TargetPosition target in FindObjectsOfType<TargetPosition>())
            Destroy(target.gameObject);

        SpawnTargets();
    }
}
