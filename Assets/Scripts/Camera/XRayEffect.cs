using DG.Tweening;
using UnityEngine;

public class HeartXRayEffect : TriggerEffect
{
    public GameObject xray;
    private GameObject heart;

    public MeshRenderer InjuredHeart;
    public MeshRenderer HealedHeart;

    void Awake()
    {
        heart = InjuredHeart.gameObject;
    }
    
    public override void OnHover()
    {
        xray.SetActive(true);
        heart.SetActive(true);
    }

    public override void OnComplete()
    {
        HealedHeart.gameObject.SetActive(true);
        InjuredHeart.material.DOFade(0f, 1.5f);
        heart =  HealedHeart.gameObject;
    }
    
    public override void OnUnhover()
    {
        heart.SetActive(false);
        xray.SetActive(false);
    }
}
