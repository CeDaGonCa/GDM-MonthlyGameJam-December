using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class MiniGameController : MonoBehaviour
{
    [Header("MiniGames")]
    public MinigameTypeController StitchMinigame;
    public MinigameTypeController SineWaveGame;
    public MinigameTypeController BarCutMinigame;
    // other games

    [Header("State")]
    public static MiniGameController Instance { get; private set; }
    private Camera mainCamera;
    private MinigameTypeController currentMinigame;

    private void Awake()
    {
        mainCamera = Camera.main;
        if (Instance != null) Debug.Log("There is already a GameController in play");
        Instance = this;
    }
    public void StartGame(MinigameData minigameData)
    {
        if (minigameData.GetType() == typeof(StitchMinigameData))
        {
            currentMinigame = StitchMinigame;
            UIController.instance.ToggleClipboard(true);
            StitchMinigame.transform.DOLocalMoveX(0, 0.5f);
            StitchMinigame.OnMinigameComplete += EndGame;
        }
        else if (minigameData.GetType() == typeof(SineWaveMinigameData))
        {
            currentMinigame = SineWaveGame;
            currentMinigame.gameObject.SetActive(true);
            UIController.instance.ToggleClipboard(true);
            SineWaveGame.transform.DOLocalMoveX(0.56f, 0.5f);
            SineWaveGame.OnMinigameComplete += EndGame;
        }
        else if (minigameData.GetType() == typeof(BarCutMinigameData))
        {
            currentMinigame = BarCutMinigame;
            currentMinigame.gameObject.SetActive(true);
            UIController.instance.ToggleClipboard(true);
            BarCutMinigame.transform.DOLocalMoveX(0, 0.5f);
            BarCutMinigame.OnMinigameComplete += EndGame;

        }
    }

    public void EndGame()
    {
        currentMinigame.OnMinigameComplete -= EndGame;
        currentMinigame.transform.DOLocalMoveX(-1.5f, 0.5f);
        UIController.instance.ToggleClipboard(false);
        GameController.Instance.ChangeState(GameState.FREECAMERA);
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
