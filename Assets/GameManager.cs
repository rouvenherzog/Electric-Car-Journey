using TurnTheGameOn.SimpleTrafficSystem;
using UnityEngine;

public enum GameState {
    PREPARING,
    RUNNING,
    END
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake() 
    { 
        if (Instance != null && Instance != this) 
            Destroy(this); 
        else 
            Instance = this; 
    }

    public GameState State = GameState.PREPARING;

    public void Update() {
        if (State != GameState.RUNNING) return;
        if (Input.GetKeyDown(KeyCode.H)) Time.timeScale = 1f;
        if (Input.GetKeyDown(KeyCode.J)) Time.timeScale = 2f;
        if (Input.GetKeyDown(KeyCode.K)) Time.timeScale = 4f;
    }

    public void StartDay() {
        if (State != GameState.PREPARING) return;
        
        State = GameState.RUNNING;
        Time.timeScale = 1f;
        
        foreach(
            WaypointIndicator indicator in
            FindObjectsOfType<WaypointIndicator>()
        ) {
            Destroy(indicator.gameObject);
        }
        AITrafficController.Instance.IsEnabled = true;
    }
}
