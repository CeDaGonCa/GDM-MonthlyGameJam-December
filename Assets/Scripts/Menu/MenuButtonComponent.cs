using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButtonComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float SlideXAmount;
    private RectTransform rectTransform;
    
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();    
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        rectTransform.DOAnchorPosX(SlideXAmount, 0.2f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rectTransform.DOAnchorPosX(0, 0.2f);
    }
}
