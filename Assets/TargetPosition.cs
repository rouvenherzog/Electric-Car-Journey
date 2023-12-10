using System;
using System.Collections;
using System.Collections.Generic;
using TurnTheGameOn.SimpleTrafficSystem;
using Unity.VisualScripting;
using UnityEngine;

public class TargetPosition : MonoBehaviour
{
    public AITrafficWaypoint Waypoint;
    public Color VisitColor;
    public Color NotVisitColor;
    public LineRenderer Line;

    public bool IsVisiting = false;
    public bool HasVisited = false;

    public void Start() {
        transform.position = Waypoint.transform.position;        
        transform.rotation =
            WaypointManager.Instance.GetWaypointRotation(Waypoint);
        
        PlayerCar.Instance.OnRouteChanged.AddListener(HandleRouteChanged);
        HandleRouteChanged();
    }

    private void OnTriggerEnter(Collider other) {
        if (HasVisited) return;

        PlayerCar player = other.GetComponent<PlayerCar>();
        if (player == null) return;

        HasVisited = true;
        player.Energy = 100f;
        UIManager.Instance.ShowEvent("Package delivered!");
    }

    private void HandleRouteChanged()
    {
        IsVisiting = PlayerCar.Instance.CalculatedRoute.Contains(Waypoint);
        Line.startColor = IsVisiting ? VisitColor : NotVisitColor;
        Line.endColor = IsVisiting ? VisitColor : NotVisitColor;
    }
}
