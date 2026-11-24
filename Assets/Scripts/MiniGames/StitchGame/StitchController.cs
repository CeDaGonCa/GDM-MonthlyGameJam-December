using System;
using UnityEngine;
using UnityEngine.UI;

public class StitchController : MinigameTypeController
{
    [Header("Values")]
    [SerializeField] int neededStitches = 4;
    
    // better practice like this is to read specifically from two children, that would give you more flexibility/source of truth    
    [Header("References")]
    public StitchRandomizer LeftSide;
    public StitchRandomizer RightSide;
    
    // State/Private Values
    int stitchCounter = 0;
    
    public static StitchController Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        minigameType = MiniGameType.Stitch;
        TimeRemaining = MiniGameTime;
    }
    
    public void StitchDone()
    {
        stitchCounter++;
        if (stitchCounter >= neededStitches)
        {
            Debug.Log("You Won");
            FinishMiniGame();
        }
    }

    public override void StartGame()
    {
        base.StartGame();
        
        stitchCounter = 0;
        LeftSide.ResetStitches();
        RightSide.ResetStitches();
    }
    
}
