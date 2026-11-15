using UnityEngine;

public class MiniGameTrigger : MonoBehaviour
{
    [SerializeField] GameObject miniGame;
    public void StartGame()
    {
        var game = Instantiate(miniGame);
        game.SetActive(true);
    }
}
