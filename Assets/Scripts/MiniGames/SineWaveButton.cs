using UnityEngine;

public class SineWaveButton : MonoBehaviour, Interactable
{
    [SerializeField] SineWave sineWave;
    [SerializeField] bool Adds;
    public void Interact()
    {
        
        sineWave.ChangeFrequency(Adds);
    }
}
