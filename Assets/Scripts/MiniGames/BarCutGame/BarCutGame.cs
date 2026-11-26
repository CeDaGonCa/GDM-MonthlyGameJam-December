
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BarCutGame : MinigameTypeController
{
    [Header("Parameters")]
    [SerializeField] float pointerSpeed = 2f;
    [SerializeField] float tolerance = 1f;
    [SerializeField] int _numOfHits = 3;
    [SerializeField] Color[] textColors = {Color.red, Color.yellow, Color.green};

    [Header("References")]
    [SerializeField] GameObject Bar;
    [SerializeField] GameObject Pointer;
    [SerializeField] GameObject Objective;
    private float barDimensions;
    private int currentHits = 0;
    private void Start()
    {
        barDimensions = Bar.transform.localScale.y / 2;
    }
    private int direction = 1;

    public override void StartGame()
    {
        base.StartGame();
        RandomizeObjective();
        base.MiniGameInstructions.text = $"{Instructions} {currentHits}/{_numOfHits} hits";
        currentHits = 0;
        UpdateColor();
        Debug.Log("Starting game");
    }
    private void RandomizeObjective()
    {
        Vector3 tmp = Objective.transform.localPosition;
        tmp.y = Random.Range(-1 * barDimensions, barDimensions);
        Objective.transform.localPosition = tmp;
    }

    protected override void Update()
    {
        base.Update();
        
        Pointer.transform.localPosition += direction * pointerSpeed * Time.deltaTime * Vector3.up;
        if (Pointer.transform.localPosition.y > barDimensions)
        {
            direction = -1;
        }
        if (Pointer.transform.localPosition.y < -1 * barDimensions)
        {
            direction = 1;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if ((Pointer.transform.localPosition - Objective.transform.localPosition).magnitude <= tolerance)
            {
                Hit();
            }
            else
            {
                Debug.Log("Bad hit");
            }
        }
    }
    void UpdateColor()
    {
        float progress = 1.0f * currentHits / _numOfHits;
        MiniGameInstructions.color = textColors[ Mathf.RoundToInt(progress * textColors.Length)];
    }
    void Hit()
    {
        currentHits++;
        base.MiniGameInstructions.text = $"{Instructions} {currentHits}/{_numOfHits} hits";
        if (currentHits >= _numOfHits)
        {
            GameWon();
        }
        else
        {
            UpdateColor();
            RandomizeObjective();
        }
    }
    void GameWon()
    {
        Debug.Log("You won");
        FinishMiniGame();
    }
}
