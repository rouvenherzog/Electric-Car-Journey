using TurnTheGameOn.SimpleTrafficSystem;
using UnityEngine;

public class LeftTurnPriority : MonoBehaviour
{
    public AITrafficStop StopSignal;

    public TriggerStatus LeadingCollider;
    public TriggerStatus ApproachingCollider;

    public void Update() {
        if(LeadingCollider.IsOccupied || ApproachingCollider.IsOccupied)
            StopSignal.StopTraffic();
        else
            StopSignal.AllowCarToProceed();
    }
}
