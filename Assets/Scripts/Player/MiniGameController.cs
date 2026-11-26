using System;
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
    public float CameraZoomAmount;
    private Vector3 originalCameraPos;
    
    [Header("MiniGames")]
    public MinigameTypeController StitchMinigame;
    public MinigameTypeController SineWaveGame;
    public MinigameTypeController BarCutMinigame;

    [Header("State")]
    public static MiniGameController Instance { get; private set; } // Singleton
    private Camera mainCamera;
    
    private MinigameTypeController currentMinigame;
    private MiniGameTrigger currentTrigger;
    
    public Action<MiniGameTrigger> OnMiniGameSuccessfullyCompleted;

    private void Awake()
    {
        mainCamera = Camera.main;
        if (Instance != null) Debug.Log("There is already a GameController in play");
        Instance = this;
        
        // subscribe once at the start for each
        StitchMinigame.OnMinigameCompleteStarted += CompleteMiniGameStarted;
        StitchMinigame.OnMinigameComplete += CompleteMinigame;
        
        SineWaveGame.OnMinigameCompleteStarted += CompleteMiniGameStarted;
        SineWaveGame.OnMinigameComplete += CompleteMinigame;
        
        BarCutMinigame.OnMinigameCompleteStarted += CompleteMiniGameStarted;
        BarCutMinigame.OnMinigameComplete += CompleteMinigame;
    }
    
    public void StartGame(MiniGameTrigger trigger)
    {
        UIController.instance.ToggleMiniGame(true);
        currentTrigger = trigger;

        Transform cam = Camera.main.transform;
        originalCameraPos = cam.position;
        Vector3 target = cam.transform.position + cam.transform.forward * CameraZoomAmount;
        cam.transform.DOMove(target, 0.3f).SetEase(Ease.InOutSine);
        
        if (trigger.MinigameData.GetType() == typeof(StitchMinigameData))
        {
            currentMinigame = StitchMinigame;
            StitchMinigame.transform.DOLocalMoveX(StitchMinigame.XPos, 0.5f);
            StitchMinigame.StartGame();
        }
        else if (trigger.MinigameData.GetType() == typeof(SineWaveMinigameData))
        {
            currentMinigame = SineWaveGame;
            currentMinigame.gameObject.SetActive(true);
            UIController.instance.ToggleMiniGame(true);
            SineWaveGame.transform.DOLocalMoveX(SineWaveGame.XPos, 0.5f);
            currentMinigame.StartGame();
        }
        else if (trigger.MinigameData.GetType() == typeof(BarCutMinigameData))
        {
            currentMinigame = BarCutMinigame;
            currentMinigame.gameObject.SetActive(true);
            UIController.instance.ToggleMiniGame(true);
            BarCutMinigame.transform.DOLocalMoveX(BarCutMinigame.XPos, 0.5f);
            currentMinigame.StartGame();
        }
    }

    private void CompleteMiniGameStarted()
    {
        currentTrigger.CompleteTrigger();
    }
    
    private void CompleteMinigame(MiniGameType type, bool success){
        currentMinigame.transform.DOLocalMoveX(-1.5f, 0.5f);
        UIController.instance.ToggleMiniGame(false);
        Camera.main.transform.DOMove(originalCameraPos, 0.3f).SetEase(Ease.InOutSine);
        GameController.Instance.ChangeState(GameState.FREECAMERA);
        
        // failed minigame, lose some health
        if (!success)
        {
            GameController.Instance.LoseTime();
        }
        else
        {
            OnMiniGameSuccessfullyCompleted?.Invoke(currentTrigger);
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
