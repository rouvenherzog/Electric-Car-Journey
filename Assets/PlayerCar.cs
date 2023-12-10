using System.Collections.Generic;
using TurnTheGameOn.SimpleTrafficSystem;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class PlayerCar : MonoBehaviour
{
    public static PlayerCar Instance { get; private set; }
    public UnityEvent OnRouteChanged = new();

    public float Energy = 100f;
    public float EnergyLossPerSecond = 1f;

    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.
        
        if (Instance != null && Instance != this) 
            Destroy(this); 
        else 
            Instance = this; 
    }

    public AITrafficWaypoint StartWaypoint;
    public List<AITrafficWaypoint> SelectedWaypoints = new();

    public List<AITrafficWaypoint> CalculatedRoute = new();

    public LineRenderer PathDrawer;

    private Vector3 StartPosition;
    private Quaternion StartRotation;

    // Start is called before the first frame update
    void Start()
    {
        StartPosition = transform.position;
        StartRotation = transform.rotation;

        AITrafficCar car = GetComponent<AITrafficCar>();
        AITrafficController.Instance.RegisterCarAI(
            car,
            StartWaypoint.onReachWaypointSettings.parentRoute
        );
    }

    public void Update() {
        if (GameManager.Instance.State != GameState.RUNNING) return;

        Energy -= Time.deltaTime * EnergyLossPerSecond;
        if (
            Energy <= 0 ||
            TargetManager.Instance.Targets.All(target => target.HasVisited)
        ) {
            Time.timeScale = 0f;
            GameManager.Instance.State = GameState.END;
        }
    }

    public void AddWaypoint(AITrafficWaypoint waypoint) {
        SelectedWaypoints.Add(waypoint);
        RefreshRoute();
    }

    public void RemoveWaypoint(AITrafficWaypoint waypoint) {
        SelectedWaypoints.Remove(waypoint);
        RefreshRoute();
    }

    private void RefreshRoute() {
        CalculatedRoute.Clear();
        List<AITrafficWaypoint> validWayPoints =
            SelectedWaypoints.Where(node => node != null).ToList();
        if (StartWaypoint != null) validWayPoints.Insert(0, StartWaypoint);
        
        for(int i = 0; i < validWayPoints.Count - 1; i++) {
            AITrafficWaypoint start = validWayPoints[i];
            AITrafficWaypoint next = validWayPoints[i+1];
            CalculatedRoute.AddRange(PathFinder.ShortestPath(start, next));
        }

        PathDrawer.positionCount = CalculatedRoute.Count;
        PathDrawer.SetPositions(
            CalculatedRoute
                .Select(waypoint =>
                    waypoint.transform.position + Vector3.up * 1.5f
                )
                .ToArray()
        );
        OnRouteChanged.Invoke();
    }

    public AITrafficWaypoint GetNextTrafficWaypoint(AITrafficWaypoint currentNode, AITrafficWaypoint[] options) {
        int currentIndex = CalculatedRoute.IndexOf(currentNode);
        CalculatedRoute.RemoveRange(0, currentIndex + 1);

        return options.First(node => node == CalculatedRoute.First());
    }

    public void Reset() {
        transform.position = StartPosition;
        transform.rotation = StartRotation;

        Energy = 100f;
        SelectedWaypoints.Clear();
        CalculatedRoute.Clear();
        RefreshRoute();

        AITrafficCar car = GetComponent<AITrafficCar>();
        AITrafficController.Instance.EnableCar(
            car.assignedIndex,
            StartWaypoint.onReachWaypointSettings.parentRoute
        );
    }
}
