using UnityEngine;

public class MiniGameTrigger : MonoBehaviour
{
    [SerializeField] GameObject miniGame;
    public void StartGame()
    {
        var game = Instantiate(miniGame,GameController.Instance.HUD.transform);
        game.SetActive(true);
    }
}
