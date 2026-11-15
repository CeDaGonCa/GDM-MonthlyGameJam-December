using UnityEngine;
using UnityEngine.InputSystem;

public class MiniGameController : MonoBehaviour
{
    private Camera mainCamera;
    private void Awake()
    {
        mainCamera = Camera.main;
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
}
