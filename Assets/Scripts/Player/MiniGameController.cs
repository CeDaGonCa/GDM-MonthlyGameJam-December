using UnityEngine;
using UnityEngine.InputSystem;

public class MiniGameController : MonoBehaviour
{
    [SerializeField] GameObject miniGameSection;
    [SerializeField] Camera miniGameCamera;

    public static MiniGameController Instance { get; private set; }
    private Camera mainCamera;
    private void Awake()
    {
        mainCamera = Camera.main;
        if (Instance != null) Debug.Log("There is already a GameController in play");
        Instance = this;
    }
    public void StartGame()
    {
        mainCamera.gameObject.SetActive(false);
        miniGameSection.SetActive(true);
    }
    public void EndGame()
    {
        miniGameSection.SetActive(false);
        mainCamera.gameObject.SetActive(true);
    }
    public void HandleUpdate()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            DetectObject();
        }
    }
    private void DetectObject()
    {
        var mousePosition = Mouse.current.position.ReadValue();
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider != null)
            {
                hit.collider.GetComponent<Interactable>()?.Interact();
            }
        }
    }
    public Camera MiniGameCamera => miniGameCamera;
}
