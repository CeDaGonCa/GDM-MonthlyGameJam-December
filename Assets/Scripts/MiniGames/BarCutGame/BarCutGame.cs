using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BarCutGame : MinigameTypeController
{
    [Header("Parameters")]
    [SerializeField] float pointerSpeed = 2f;
    [SerializeField] float tolerance = 1f;

    [Header("References")]
    [SerializeField] GameObject Bar;
    [SerializeField] GameObject Pointer;
    [SerializeField] GameObject Objective;
    private float barDimensions;
    private void Start()
    {
        barDimensions = Bar.transform.localScale.y / 2;
    }
    private int direction = 1;
    bool gameWon = false;

    public override void StartGame()
    {
        base.StartGame();
        
        Debug.Log("Starting game");
    }

    void Update()
    {
        if(gameWon) return;
        Pointer.transform.localPosition += direction * pointerSpeed * Time.deltaTime * Vector3.up;
        if(Pointer.transform.localPosition.y > barDimensions)
        {
            direction = -1;
        }
        if(Pointer.transform.localPosition.y < -1 * barDimensions)
        {
            direction = 1;
        }
        
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if ((Pointer.transform.localPosition - Objective.transform.localPosition).magnitude <= tolerance)
            {
                GameWon();
            }
            else
            {
                Debug.Log("Bad hit");
            }
        }
    }
    void GameWon()
    {
        Debug.Log("You won");
        gameWon = true;
        FinishMiniGame();
    }
}
