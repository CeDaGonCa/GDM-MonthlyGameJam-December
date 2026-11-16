using System;
using UnityEngine;

public class MiniGameTrigger : MonoBehaviour
{
    [SerializeField] GameObject miniGame;

    public Action<GameObject> OnGameStart;
    public void StartGame()
    {
        OnGameStart.Invoke(miniGame);
    }
}
