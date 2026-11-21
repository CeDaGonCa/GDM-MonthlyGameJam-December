using System;
using UnityEngine;

public class MiniGameTrigger : MonoBehaviour
{
    [SerializeField] MinigameData _minigameData;
    public Action<MinigameData> OnGameStart;
    
    public void StartGame()
    {
        OnGameStart.Invoke(_minigameData);
    }
}
