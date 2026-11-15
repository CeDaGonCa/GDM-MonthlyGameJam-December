using UnityEngine;

public class StitchController : MonoBehaviour
{
    [SerializeField] int neededStitches = 4;
    public static StitchController Instance {get; private set;}

    void Awake()
    {
        Instance = this;
    }
    int stitchCounter = 0;
    public void StitchDone()
    {
        stitchCounter++;
        if(stitchCounter >= neededStitches)
        {
            Debug.Log("You Won");
        }
    }
}
