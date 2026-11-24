using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

// Overarching "GameController/Manager", controls current game state (controller) and player's status
public class GameController : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField] FreeLookCameraController cameraController;
    [SerializeField] MiniGameController miniGameController;
    
    [Header("Triggers")]
    [SerializeField] Transform miniGameTriggers;

    [Header("UI References")] // dont put UI refs in managers usually, but its a gamejam so wtv
    public TextMeshProUGUI TimerText;
    
    [Header("Design Values")]
    public float TimeForLevel = 180; //TODO - maybe trace this from somewhere to change per level

    // State
    public float TimeRemaining;
    
    public static GameController Instance { get; private set; }
    public GameState State { get; private set; }
    
    void Awake()
    {
        if (Instance != null) Debug.Log("There is already a GameController in play");
        Instance = this;
    }
    
    void Start()
    {
        State = GameState.FREECAMERA;
        cameraController.HandleStart();
        
        TimeRemaining = TimeForLevel;
        for (int i = 0; i < miniGameTriggers.childCount; i++) // could prolly use Resources.FindAllByType or whatever that func is 
        {
            var trigger = miniGameTriggers.GetChild(i).GetComponent<MiniGameTrigger>();
            if (trigger == null) Debug.Log("NON TRIGGER IN TRIGGER POOL");
            trigger.OnGameStart += GameStart;
        }
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
    
    public void ChangeState(GameState newState)
    {
        // prolly would want to do an actual statemachine type of thing
        if (newState == State) 
            return; // returning if the same state
        
        if (newState == GameState.MINIGAME)
            Cursor.lockState = CursorLockMode.Confined;
        else if (newState == GameState.FREECAMERA)
            Cursor.lockState = CursorLockMode.Locked;
        
        State = newState;
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
    
    private void GameStart(MinigameTaskData minigameData)
    {
        ChangeState(GameState.MINIGAME);
        miniGameController.StartGame(minigameData);
    }
    
    public List<MinigameTaskData> GetAllTasks()
    {
        MiniGameTrigger[] Triggers = Resources.FindObjectsOfTypeAll<MiniGameTrigger>();

        List<MinigameTaskData> Tasks = new();
        foreach (MiniGameTrigger Trigger in Triggers)
        {
            Tasks.Add(Trigger.MinigameData);
        }
    
        return Tasks;
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