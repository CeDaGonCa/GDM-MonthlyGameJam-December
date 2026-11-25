using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MiniGameInstance
{
    public bool Completed;
    public TaskWidget Task;
}

// Overarching "GameController/Manager", controls current game state (controller) and player's status
public class GameController : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField] FreeLookCameraController cameraController;
    [SerializeField] MiniGameController miniGameController;
    
    [Header("Triggers")]
    [SerializeField] Transform miniGameTriggers;
    private Dictionary<MiniGameTrigger, MiniGameInstance> MinigameCompletion = new();

    [Header("UI References")] // dont put UI refs in managers usually, but its a gamejam so wtv
    public CanvasGroup HighlightsHUD;
    public TextMeshProUGUI TimerText;
    public TaskListController taskListController;
    
    [Header("Design Values")]
    public float TimeForLevel = 180;

    // State
    public float TimeRemaining;
    
    public static GameController Instance { get; private set; }
    public GameState State { get; private set; }
    
    void Awake()
    {
        if (Instance != null) Debug.Log("There is already a GameController in play");
        Instance = this;

        foreach (MiniGameTrigger Trigger in Resources.FindObjectsOfTypeAll<MiniGameTrigger>())
        {
            Trigger.OnGameStart += GameStart;
            TaskWidget task = taskListController.CreateTask(Trigger.MinigameData);
            MinigameCompletion.Add(Trigger, new MiniGameInstance(){Completed = false, Task = task});
        }
        
        miniGameController.OnMiniGameSuccessfullyCompleted += OnCompletedMiniGame;
    }
    
    void Start()
    {
        State = GameState.FREECAMERA;
        cameraController.HandleStart();
        
        TimeRemaining = TimeForLevel;
    }
    
    void Update()
    {
        //@Cesar Consider using state machines instead
        if (State == GameState.MINIGAME)
        {
            miniGameController.HandleUpdate();
        }
        else if (State == GameState.FREECAMERA)
        {
            //@Cesar change this to be a controller instead for consistency
            cameraController.HandleUpdate();
        }
        
        // increment timer/life
        TimeRemaining -= Time.deltaTime;
        TimerText.text = FormatTime(TimeRemaining);
    }

    private void OnCompletedMiniGame(MiniGameTrigger trigger)
    {
        if (trigger == null || !MinigameCompletion.ContainsKey(trigger))
            return;

        MinigameCompletion[trigger].Completed = true;
        MinigameCompletion[trigger].Task.MarkComplete(true);
    }
    
    public void ChangeState(GameState newState)
    {
        // prolly would want to do an actual statemachine type of thing
        if (newState == State) 
            return; // returning if the same state

        IEnumerator SwitchState()
        {
            yield return null;
            
            if (newState == GameState.MINIGAME)
                Cursor.lockState = CursorLockMode.Confined;
            else if (newState == GameState.FREECAMERA)
                Cursor.lockState = CursorLockMode.Locked;
        
            HighlightsHUD.DOFade(newState == GameState.MINIGAME ? 0 : 1, 0.2f);
            State = newState;
        }
        
        StartCoroutine(SwitchState());
    }

    // ignore this, this UI script should NOT be in manager, have more layers usually but for this idrc
    public void LoseTime(float duration = 1f, int flashes = 3)
    {
        TimeRemaining -= 10f;
        TimerText.DOKill();                 // stop any flash already running
        TimerText.color = Color.white;
        
        float half = duration / (flashes * 2f);

        Sequence seq = DOTween.Sequence().SetTarget(TimerText);
        for (int i = 0; i < flashes; i++)
        {
            seq.Append(TimerText.DOColor(Color.red, half));
            seq.Append(TimerText.DOColor(Color.white, half));
        }
    }
    
    private void GameStart(MiniGameTrigger trigger)
    {
        ChangeState(GameState.MINIGAME);
        miniGameController.StartGame(trigger);
    }

    string FormatTime(float seconds)
    {
        int total = Mathf.Max(0, Mathf.FloorToInt(seconds));
        int minutes = total / 60;
        int secs = total % 60;
        return $"{minutes:00}:{secs:00}";
    }
}

public enum GameState
{
    MINIGAME, FREECAMERA
}