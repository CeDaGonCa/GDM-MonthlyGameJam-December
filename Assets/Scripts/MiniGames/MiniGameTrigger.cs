using System;
using UnityEngine;

public class MiniGameTrigger : MonoBehaviour
{
    [SerializeField] MinigameTaskData _minigameData;
    public MinigameTaskData MinigameData => _minigameData;
        
    public Action<MinigameTaskData> OnGameStart;
    
    public void StartGame()
    {
        OnGameStart.Invoke(_minigameData);
    }
}
