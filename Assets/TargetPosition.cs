using TurnTheGameOn.SimpleTrafficSystem;
using UnityEngine;
using UnityEngine.Events;

public class TargetPosition : MonoBehaviour
{
    public UnityEvent OnVisit = new();
    public AITrafficWaypoint Waypoint;
    public Color VisitColor;
    public Color NotVisitColor;
    public LineRenderer Line;

    public bool IsVisiting = false;
    public bool HasVisited = false;
    public string VisitedText = "";

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
        OnVisit.Invoke();
        if (VisitedText != "")
            UIManager.Instance.ShowEvent(VisitedText);
    }

    private void HandleRouteChanged()
    {
        IsVisiting = PlayerCar.Instance.CalculatedRoute.Contains(Waypoint);
        Line.startColor = IsVisiting ? VisitColor : NotVisitColor;
        Line.endColor = IsVisiting ? VisitColor : NotVisitColor;
    }
}
