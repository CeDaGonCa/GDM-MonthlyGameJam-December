using UnityEngine;

public class StitchRandomizer : MonoBehaviour
{
    void Awake()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            int newSpot = Random.Range(0,transform.childCount);
            // Swapping values
            (transform.GetChild(newSpot).position, transform.GetChild(i).position) = (transform.GetChild(i).position, transform.GetChild(newSpot).position);
        }
    }
}
