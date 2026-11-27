using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MiniGameTrigger : MonoBehaviour
{
    public List<TriggerEffect> triggerEffects;
    [SerializeField] MinigameTaskData _minigameData;
    public MinigameTaskData MinigameData => _minigameData;

    [SerializeField] private bool FadeAfterCompletion = false;
    [SerializeField] private bool _isActive = true;
    [SerializeField] private MeshRenderer renderer;
    public bool IsActive => _isActive;

    public Action<MiniGameTrigger> OnGameStart;
    
    public void StartGame()
    {
        OnGameStart.Invoke(this);
    }

    public void CompleteTrigger()
    {
        _isActive = false;
        if (FadeAfterCompletion)
            renderer.material.DOFade(0f, 2f);

        foreach (TriggerEffect triggerEffect in triggerEffects)
        {
            triggerEffect.OnComplete();
        }
    }

    public void OnHover()
    {
        foreach (TriggerEffect triggerEffect in triggerEffects)
        {
            triggerEffect.OnHover();
        }
    }

    public void OnUnhover()
    {
        foreach (TriggerEffect triggerEffect in triggerEffects)
        {
            triggerEffect.OnUnhover();
        }
    }
}
