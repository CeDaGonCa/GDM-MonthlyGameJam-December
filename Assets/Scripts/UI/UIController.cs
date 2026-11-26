using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    
    [Header("UI Elements")]
    [SerializeField] CanvasGroup ClipboardCanvasGroup;
    [SerializeField] RectTransform Hands;
    [SerializeField] RectTransform Timer;
    
    public TaskListController  taskListController;
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI OpeningText;
    public CanvasGroup ShownInGameHUD;

    void Awake()
    {
        instance = this;
    }

    public void ToggleMiniGame(bool hide)
    {
        ClipboardCanvasGroup.DOFade(hide ? 0 : 1, 0.3f);
        Hands.DOAnchorPosY(hide ? -400 : 0, 0.3f);
    }

    public void StopUI()
    {
        ClipboardCanvasGroup.DOFade(0, 0.6f);
        Hands.DOAnchorPosY(-400, 0.6f);
        Timer.DOAnchorPosY(400, 0.6f);
    }
}
