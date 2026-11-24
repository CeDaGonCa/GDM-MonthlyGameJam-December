using UnityEngine;

public class SineWaveButton : MonoBehaviour, Interactable
{
    [SerializeField] SineWave sineWave;
    [SerializeField] bool Adds;
    [SerializeField] bool changesFreq;
    public void Interact()
    {
        Debug.Log("Interacted on raycast THIS SHOULDN'T HAPPEN");
        sineWave.ChangeValues(Adds,changesFreq);
    }
    void OnMouseDown()
    {
        Debug.Log("Interacted on mouse down");
        sineWave.ChangeValues(Adds,changesFreq);
    }
}
