using DG.Tweening;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    
    [Header("UI Elements")]
    [SerializeField] CanvasGroup ClipboardCanvasGroup;

    void Awake()
    {
        instance = this;
    }

    public void ToggleClipboard(bool hide)
    {
        ClipboardCanvasGroup.DOFade(hide ? 0 : 1, 0.3f);
    }
}
