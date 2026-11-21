using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] FreeLookCameraController cameraController;
    [SerializeField] MiniGameController miniGameController;
    
    [SerializeField] Transform miniGameSection;
    [SerializeField] Transform miniGameTriggers;
    public GameObject HUD;

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
        for (int i = 0; i < miniGameTriggers.childCount; i++)
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
    }
    public void ChangeState(GameState newState)
    {
        //@Cesar same thing - you'd want to use a state machine here, so that you can have a generic "OnStateEnter, OnUpdate, OnStateExit" functions to call
        // this makes it much more easily iterable and to be built on
        
        if (newState == State) 
            return; // returning if the same state
        
        if (newState == GameState.MINIGAME)
        {
            Cursor.lockState = CursorLockMode.Confined;
            // cameraController.
        }
        else if (newState == GameState.FREECAMERA)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        State = newState;
    }
    
    private void GameStart(MinigameData minigameData)
    {
        ChangeState(GameState.MINIGAME);
        miniGameController.StartGame(minigameData);
    }
}
public enum GameState
{
    MINIGAME, FREECAMERA
}
