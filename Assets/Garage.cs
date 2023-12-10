using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AOT;
using TurnTheGameOn.SimpleTrafficSystem;

public class Garage : MonoBehaviour
{
    public AITrafficCar CarPrefab;
    public AITrafficSpawnPoint SpawnPoint;
    public AITrafficWaypointRoute Route;
    public int AmountCarsToSpawn = 10;

    public AITrafficCar LastSpawnedCar = null;
    public int SpawnedCars = 0;

    void Update()
    {
        if (SpawnedCars >= AmountCarsToSpawn) return;
        if(LastSpawnedCar && Vector3.Distance(LastSpawnedCar.transform.position, SpawnPoint.waypoint.transform.position) < 6) return;

        LastSpawnedCar = Instantiate(CarPrefab, SpawnPoint.waypoint.transform.position + Vector3.up * 0.1f, SpawnPoint.transform.rotation, AITrafficController.Instance.transform);
        LastSpawnedCar.RegisterCar(Route);
        SpawnedCars++;
    }
}
