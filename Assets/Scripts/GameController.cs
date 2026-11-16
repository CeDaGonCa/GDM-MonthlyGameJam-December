using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] FreeLookCamera camera;
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
        camera.HandleStart();
        for (int i = 0; i < miniGameTriggers.childCount; i++)
        {
            var trigger = miniGameTriggers.GetChild(i).GetComponent<MiniGameTrigger>();
            if (trigger == null) Debug.Log("NON TRIGGER IN TRIGGER POOL");
            trigger.OnGameStart += GameStart;
        }
    }
    void Update()
    {
        if (State == GameState.MINIGAME)
        {
            miniGameController.HandleUpdate();
        }
        else if (State == GameState.FREECAMERA)
        {
            camera.HandleUpdate();
        }
    }
    public void ChangeState(GameState newState)
    {
        if (newState == State) return;
        if (newState == GameState.MINIGAME)
        {
            Cursor.lockState = CursorLockMode.Confined;
            miniGameController.StartGame();
        }
        else if (newState == GameState.FREECAMERA)
        {
            Cursor.lockState = CursorLockMode.Locked;
            miniGameController.EndGame();
        }
        State = newState;
    }
    private void GameStart(GameObject miniGame)
    {
        var game = Instantiate(miniGame, miniGameSection);
        game.SetActive(true);
    }
}
public enum GameState
{
    MINIGAME, FREECAMERA
}
