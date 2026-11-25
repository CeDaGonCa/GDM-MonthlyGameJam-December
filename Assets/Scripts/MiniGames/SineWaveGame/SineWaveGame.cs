using UnityEngine;

public class SineWaveGame : MinigameTypeController
{
    [SerializeField] float tolerance = 0.02f;
    [SerializeField] float completionTime = 5f;
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
        ReferenceWave.RandomizeWave(); // Should I add something to check that the game is not instantly won? the odds are super small tho :/
        PlayerWave.RandomizeWave();
    }
}
