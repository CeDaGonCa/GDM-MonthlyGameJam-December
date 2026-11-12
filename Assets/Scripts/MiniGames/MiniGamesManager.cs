using UnityEngine;

public class MiniGamesManager : MonoBehaviour
{
    [SerializeField] SineWaveGame sineWaveGame;
    public void StartSineWaveGame()
    {
        sineWaveGame.gameObject.SetActive(true);
        sineWaveGame.StartGame();
    }
}
