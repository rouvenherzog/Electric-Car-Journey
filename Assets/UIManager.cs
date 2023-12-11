using System.Collections.Generic;
using System.Linq;
using TMPro;
using TurnTheGameOn.SimpleTrafficSystem;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
        private void Awake() 
    { 
        if (Instance != null && Instance != this) 
            Destroy(this); 
        else 
            Instance = this; 
    }

    public GameObject GameEndUI;
    public TextMeshProUGUI GameEndText;
    public Button RegenerateButton;
    public Button RetryButton;
    
    public GameObject PreparationUI;
    public TextMeshProUGUI PreparationText;
    public Button StartButton;
    
    public GameObject PlayingUI;
    public Image EnergyProgress;
    public Transform EventTextWrapper;
    public EventText EventTextPrefab;
    public Button Speed1XButton;
    public Button Speed2XButton;
    public Button Speed4XButton;

    public void Start() {
        PreparationUI.SetActive(true);
        PlayingUI.SetActive(false);
        GameEndUI.SetActive(false);

        Speed1XButton.onClick.AddListener(() => Time.timeScale = 1f);
        Speed2XButton.onClick.AddListener(() => Time.timeScale = 2f);
        Speed4XButton.onClick.AddListener(() => Time.timeScale = 4f);
        StartButton.onClick.AddListener(() => GameManager.Instance.StartDay());

        RegenerateButton.onClick.AddListener(HandleRegenerate);
        RetryButton.onClick.AddListener(HandleRetry);
    }

    private void HandleRetry()
    {
        foreach(AITrafficCar car in FindObjectsOfType<AITrafficCar>()) {
            if (car.gameObject == PlayerCar.Instance.gameObject)
                continue;

            AITrafficController.Instance.MoveCarToPool(car.assignedIndex);
        }

        foreach(Garage garage in FindObjectsOfType<Garage>())
            garage.Reset();

        PlayerCar.Instance.Reset();
        GameManager.Instance.State = GameState.PREPARING;
    }

    private void HandleRegenerate()
    {
        TargetManager.Instance.Regenerate();
        HandleRetry();
    }

    public void Update() {
        switch(GameManager.Instance.State) {
            case GameState.PREPARING:
                UpdatePreparationUI();
                break;
            case GameState.RUNNING:
                UpdateRunningUI();
                break;
            case GameState.END:
                UpdateEndUI();
                break;
        }
    }

    private void UpdateEndUI()
    {
        
        PreparationUI.SetActive(false);
        PlayingUI.SetActive(false);
        GameEndUI.SetActive(true);

        List<TargetPosition> targets = TargetManager.Instance.Targets;
        int visited = targets.Where(target => target.HasVisited).Count();
        int total = targets.Count();
        if (visited >= total)
            GameEndText.text = $"Yay!\nYou delivered all packages.";
        else
            GameEndText.text = $"Game Over!\nYou ran out of power before delivering all packages.";

    }

    private void UpdateRunningUI()
    {
        PreparationUI.SetActive(false);
        PlayingUI.SetActive(true);
        GameEndUI.SetActive(false);

        EnergyProgress.fillAmount = PlayerCar.Instance.Energy / 100f;
    }

    private void UpdatePreparationUI()
    {
        PreparationUI.SetActive(true);
        PlayingUI.SetActive(false);
        GameEndUI.SetActive(false);

        List<TargetPosition> targets = TargetManager.Instance.Targets;
        int connected = targets.Where(target => target.IsVisiting).Count();
        int total = targets.Count();
        PreparationText.text = $"Connected {connected} out of {total} stops.";

        StartButton.interactable = connected >= total;
    }

    public void ShowEvent(string text) {
        EventText eventText = Instantiate(EventTextPrefab, EventTextWrapper);
        eventText.Text.text = text;
    }
}
