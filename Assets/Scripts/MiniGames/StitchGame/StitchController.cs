using System;
using UnityEngine;

public class StitchController : MinigameTypeController
{

    [SerializeField] int neededStitches = 4;
    
    // better practice like this is to read specifically from two children, that would give you more flexibility/source of truth    
    public StitchRandomizer LeftSide;
    public StitchRandomizer RightSide;
    
    public static StitchController Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }
    int stitchCounter = 0;
    public void StitchDone()
    {
        stitchCounter++;
        if (stitchCounter >= neededStitches)
        {
            Debug.Log("You Won");
            OnMinigameComplete?.Invoke();
        }
    }

    public void RestartGame()
    {
        LeftSide.Randomize();
        RightSide.Randomize();
    }
}
