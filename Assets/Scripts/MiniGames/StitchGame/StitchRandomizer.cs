using UnityEngine;

// Parent controller of one "side" of stitches
public class StitchRandomizer : MonoBehaviour
{
    void Start()
    {
        ResetStitches();
    }   

    public void ResetStitches()
    {
        foreach (Stitch stitch in transform.GetComponentsInChildren<Stitch>())
        {
            stitch.ResetStitch();
        }
        
        for(int i = 0; i < transform.childCount; i++)
        {
            int newSpot = Random.Range(0,transform.childCount);
            // Swapping values
            (transform.GetChild(newSpot).position, transform.GetChild(i).position) = (transform.GetChild(i).position, transform.GetChild(newSpot).position);
        }
    }
}
