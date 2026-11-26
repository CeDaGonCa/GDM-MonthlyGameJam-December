using System;
using UnityEngine;

public class MiniGameTrigger : MonoBehaviour
{
    [SerializeField] MinigameTaskData _minigameData;
    public MinigameTaskData MinigameData => _minigameData;

    [SerializeField] private bool _isActive = true;
    public bool IsActive => _isActive;
    
    public Action<MiniGameTrigger> OnGameStart;
    
    public void StartGame()
    {
        OnGameStart.Invoke(this);
    }

    public void CompleteTrigger()
    {
        _isActive = false;
    }
}
