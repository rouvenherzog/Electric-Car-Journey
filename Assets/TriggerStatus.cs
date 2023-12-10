using System.Collections;
using System.Collections.Generic;
using TurnTheGameOn.SimpleTrafficSystem;
using UnityEngine;

public class TriggerStatus : MonoBehaviour
{
    public List<AITrafficCar> AITrafficCars = new();
    public List<AITrafficWaypointRoute> TriggeringRoutes = new();
    public bool IsOccupied = false;

    public void OnTriggerEnter(Collider other) {
        AITrafficCar car = other.GetComponent<AITrafficCar>();
        if (car == null) return;
        AITrafficWaypointRoute carRoute = 
            AITrafficController.Instance.GetCarRoute(car.assignedIndex);
        if (TriggeringRoutes.Count > 0 && !TriggeringRoutes.Contains(carRoute))
            return;

        AITrafficCars.Add(car);
        IsOccupied = true;
    }

    public void OnTriggerExit(Collider other) {
        AITrafficCar car = other.GetComponent<AITrafficCar>();
        if (car == null || !AITrafficCars.Contains(car)) return;
        AITrafficCars.Remove(car);
        IsOccupied = AITrafficCars.Count > 0;
    }

    public void OnDrawGizmos() {
        Gizmos.color = IsOccupied ? Color.red : Color.green;
        Gizmos.DrawSphere(transform.position, 1);
        Gizmos.DrawLine(
            transform.position,
            transform.position + transform.forward * -2
        );
    }
}
