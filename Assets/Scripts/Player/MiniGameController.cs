using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MiniGameType
{
    Stitch, Wave, Slider
}

// Controller when the game is in MiniGameMode, and manages which MiniGame is being displayed
public class MiniGameController : MonoBehaviour
{
    [Header("MiniGames")]
    public MinigameTypeController StitchMinigame;
    public MinigameTypeController SineWaveGame;
    public MinigameTypeController BarCutMinigame;

    [Header("State")]
    public static MiniGameController Instance { get; private set; }
    private Camera mainCamera;
    private MinigameTypeController currentMinigame;

    private void Awake()
    {
        mainCamera = Camera.main;
        if (Instance != null) Debug.Log("There is already a GameController in play");
        Instance = this;
        
        // subscribe once at the start for each
        StitchMinigame.OnMinigameComplete += CompleteMinigame;
        // WaveMinigame.OnMinigameComplete += CompleteMinigame;
        // SliderMinigame.OnMinigameComplete += CompleteMinigame;
    }
    
    public void StartGame(MinigameTaskData minigameData)
    {
        UIController.instance.ToggleMiniGame(true);
        
        if (minigameData.GetType() == typeof(StitchMinigameData))
        {
            currentMinigame = StitchMinigame;
            StitchMinigame.transform.DOLocalMoveX(0, 0.5f);
            StitchMinigame.StartGame();
        }
        else if (minigameData.GetType() == typeof(SineWaveMinigameData))
        {
            currentMinigame = SineWaveGame;
            currentMinigame.gameObject.SetActive(true);
            UIController.instance.ToggleMiniGame(true);
            SineWaveGame.transform.DOLocalMoveX(0.56f, 0.5f);
        }
        else if (minigameData.GetType() == typeof(BarCutMinigameData))
        {
            currentMinigame = BarCutMinigame;
            currentMinigame.gameObject.SetActive(true);
            UIController.instance.ToggleMiniGame(true);
            BarCutMinigame.transform.DOLocalMoveX(0, 0.5f);
        }
    }

    private void CompleteMinigame(MiniGameType type, bool success){
        currentMinigame.transform.DOLocalMoveX(-1.5f, 0.5f);
        UIController.instance.ToggleMiniGame(false);
        GameController.Instance.ChangeState(GameState.FREECAMERA);
        
        // failed minigame, lose some health
        if (!success)
        {
            GameController.Instance.LoseTime();
        }
    }

    public void HandleUpdate()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            DetectObject();
        }
    }
    private void DetectObject()
    {
        var mousePosition = Mouse.current.position.ReadValue();
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider != null)
            {
                hit.collider.GetComponent<Interactable>()?.Interact();
            }
        }
    }
}
