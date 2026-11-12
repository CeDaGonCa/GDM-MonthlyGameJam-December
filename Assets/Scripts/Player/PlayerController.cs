using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    private Camera mainCamera;
    InputAction clickAction;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        //Cursor.lockState = CursorLockMode.Confined;
        clickAction = playerInput.actions["Click"];
        //clickAction.performed += _ => DetectObject();
        mainCamera = Camera.main;
    }
    private void Update()
    {
        if (clickAction.WasPressedThisFrame())
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
