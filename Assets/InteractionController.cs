using TurnTheGameOn.SimpleTrafficSystem;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public GameObject WayRouteIndicator;
    
    private Vector3 LastClickPosition;
    private Ray LastClick;

    private void Update() {
        if (GameManager.Instance.State == GameState.PREPARING) CheckClick();
        CheckStartDay();
    }

    private void CheckStartDay() {
        if (!Input.GetKeyUp(KeyCode.L)) return;

        GameManager.Instance.StartDay();
    }

    private void CheckClick() {
        if (!Input.GetMouseButtonUp(0)) return;
        LastClickPosition = Camera.main.transform.position;
        LastClick = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        int streetLayer = LayerMask.NameToLayer("Street");
        int waypointLayer = LayerMask.NameToLayer("Waypoint");
        int raycastMask = (1<<streetLayer) | (1<<waypointLayer);
        // Does the ray intersect any objects excluding the player layer
        if (!Physics.Raycast(LastClick, out hit, 500f, raycastMask)) return;

        GameObject collider = hit.collider.gameObject;
        if (collider.layer == waypointLayer)
            RemoveWaypoint(hit);
        else
            AddWaypoint(hit);
    }

    private void RemoveWaypoint(RaycastHit hit) {
        AITrafficWaypoint closestWaypoint =
            WaypointManager.Instance.GetClosestWaypoint(
                hit.collider.transform.position
            );

        PlayerCar.Instance.RemoveWaypoint(closestWaypoint);
        Destroy(hit.collider.gameObject);
    }

    private void AddWaypoint(RaycastHit hit) {
        AITrafficWaypoint closestWaypoint =
            WaypointManager.Instance.GetClosestWaypoint(hit.point);
        
        Instantiate(
            WayRouteIndicator,
            closestWaypoint.transform.position,
            Quaternion.identity
        );
        PlayerCar.Instance.AddWaypoint(closestWaypoint);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(LastClickPosition, LastClick.GetPoint(500f));
    }
}
