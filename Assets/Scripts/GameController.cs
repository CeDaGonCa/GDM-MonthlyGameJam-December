using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] FreeLookCamera camera;
    [SerializeField] MiniGameController miniGameController;
    public GameObject HUD;

    public static GameController Instance { get; private set; }
    private GameState state;
    void Awake()
    {
        if (Instance != null) Debug.Log("There is already a GameController in play");
        Instance = this;

    }
    void Start()
    {
        state = GameState.FREECAMERA;
        camera.HandleStart();
    }
    void Update()
    {
        if (state == GameState.MINIGAME)
        {
            miniGameController.HandleUpdate();
        }
        else if (state == GameState.FREECAMERA)
        {
            camera.HandleUpdate();
        }
    }
    public void ChangeState(GameState newState)
    {
        if (newState == state) return;
        Debug.Log($"Changed state to {newState}");
        if (newState == GameState.MINIGAME)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
        else if (newState == GameState.FREECAMERA)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        state = newState;
    }
}
public enum GameState
{
    MINIGAME, FREECAMERA
}
