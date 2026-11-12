using UnityEngine;

public class SineWaveGame : MonoBehaviour
{
    [SerializeField] float tolerance = 0.02f;
    [SerializeField] float completionTime = 5f;
    [SerializeField] SineWave ReferenceWave;
    [SerializeField] SineWave PlayerWave;

    float EndCompletionTime = 0f;
    void Start()
    {
        gameObject.SetActive(false);
        EndCompletionTime = Time.time + completionTime;
    }
    bool GameWon = false;
    // Update is called once per frame
    void Update()
    {
        if (GameWon) return;
        if (Mathf.Abs(ReferenceWave.Frequency - PlayerWave.Frequency) <= tolerance)
        {
            if (EndCompletionTime <= Time.time)
            {
                Debug.Log("Won game");
                GameWon = true;
            }
        }
        else
        {
            EndCompletionTime = Time.time + completionTime;
        }
    }

    public void StartGame()
    {
        // Probably something to set the Waves if needed
    }
}
