using DG.Tweening;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    
    [Header("UI Elements")]
    [SerializeField] CanvasGroup ClipboardCanvasGroup;
    [SerializeField] RectTransform Hands;

    void Awake()
    {
        instance = this;
    }

    public void ToggleMiniGame(bool hide)
    {
        ClipboardCanvasGroup.DOFade(hide ? 0 : 1, 0.3f);
        Hands.DOAnchorPosY(hide ? -400 : 0, 0.3f);
    }
}
