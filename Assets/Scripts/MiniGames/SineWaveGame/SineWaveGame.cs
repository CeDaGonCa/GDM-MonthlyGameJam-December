using System;
using UnityEngine;

public class SineWaveGame : MinigameTypeController
{
    [SerializeField] float tolerance = 0.02f;
    [SerializeField] float completionTime = 5f;
    [SerializeField] float changeAmount = 0.05f;
    [SerializeField] float minDistance = 0.4f;
    [SerializeField] SineWave ReferenceWave;
    [SerializeField] SineWave PlayerWave;

    bool GameWon = false;

    void Start()
    {
        PlayerWave.OnSineWaveChanged += OnSineWaveChanged;
        
    }
    
    void OnSineWaveChanged()
    {
        if (GameWon) return;
        Debug.Log($"References freq: {ReferenceWave.Frequency} \n Player's Freq: {PlayerWave.Frequency}");
        if (Mathf.Abs(ReferenceWave.Frequency - PlayerWave.Frequency) <= tolerance)
        {
            GameWon = true;
            FinishMiniGame();
        }
    }

    public override void StartGame()
    {
        base.StartGame();
        
        // Probably something to set the Waves if needed
        GameWon = false;
        Debug.Log("Reset the player wave");
        Debug.Log("Set the reference wave to some random");
        PlayerWave.SetUpWave(changeAmount);
        ReferenceWave.SetUpWave(changeAmount);
        float playerFreq = PlayerWave.Frequency;
        float referenceFreq = ReferenceWave.Frequency;
        while (Math.Abs(playerFreq - referenceFreq) <= minDistance)
        {
            playerFreq = PlayerWave.RandomizeWave();
            referenceFreq = ReferenceWave.RandomizeWave();
        }
    }
}
