using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Generic controller for minigames
public abstract class MinigameTypeController : MonoBehaviour
{
    [Header("Generic Minigame Values")]
    protected MiniGameType minigameType;
    public float MiniGameTime = 15f;
    public String Instructions;
    public float XPos;

    public Action OnMinigameCompleteStarted;
    public Action<MiniGameType, bool> OnMinigameComplete;
    
    [SerializeField] protected CanvasGroup MiniGameCompleteIndicator;
    [SerializeField] protected TMP_Text MiniGameInstructions;
    public Slider Timer;
    
    // Game State
    protected float TimeRemaining;
    protected bool bIsBeingPlayed = false;

    public virtual void StartGame()
    {
        MiniGameCompleteIndicator.alpha = 0;
        bIsBeingPlayed = true;
        TimeRemaining = MiniGameTime;
        Timer.value = 1;
        MiniGameInstructions.text = Instructions;
    }
    
    protected void FinishMiniGame()
    {
        bIsBeingPlayed = false;
        StartCoroutine(FinishMiniGameProcess());
    }

    // ignore this its ugly as fuck but im lazy
    IEnumerator FinishMiniGameProcess()
    {
        OnMinigameCompleteStarted?.Invoke();
        
        yield return null;
        MiniGameCompleteIndicator.DOFade(1f, 0.1f);
        yield return new WaitForSeconds(0.1f);
        MiniGameCompleteIndicator.DOFade(0.6f, 0.1f);
        yield return new WaitForSeconds(0.1f);
        MiniGameCompleteIndicator.DOFade(1f, 0.1f);
        yield return new WaitForSeconds(0.1f);
        MiniGameCompleteIndicator.DOFade(0.8f, 0.1f);
        yield return new WaitForSeconds(0.1f);
        MiniGameCompleteIndicator.DOFade(1f, 0.1f);
        yield return new WaitForSeconds(0.6f);
        
        OnMinigameComplete?.Invoke(minigameType, true);
    }
    
    // All minigames will have some sort of timer, however others might also have a second fail condition (failing the slider bar too many times, etc.)
    // All fail conditions simply fire OnMiniGameComplete(false)
    protected virtual void Update()
    {
        if (!bIsBeingPlayed) return;
        
        TimeRemaining -= Time.deltaTime;
        Timer.value = TimeRemaining / MiniGameTime;

        if (TimeRemaining <= 0.0f)
        {
            OnMinigameComplete?.Invoke(minigameType, false);
            bIsBeingPlayed = false;
        }
    }
}
