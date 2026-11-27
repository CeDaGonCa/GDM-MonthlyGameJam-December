using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CameraRaycast : MonoBehaviour
{
    public Vector2 ScaleRatio = new Vector2(1.5f, 1.3f);
    public float interactionDistance = 10f;
    public RectTransform highlight;

    private Camera cam;
    private GameObject currentTarget;
    [SerializeField] private MiniGameTrigger hoveredTrigger;

    void Start()
    {
        cam = GetComponent<Camera>();
        highlight.gameObject.SetActive(false);
    }

    public void HandleUpdate()
    {
        Ray ray = cam.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            MiniGameTrigger miniGameTrigger = hit.collider.GetComponent<MiniGameTrigger>();
            
            // if not hitting an interactable body part, set return early and set trigger to nothing
            if (!hit.collider.CompareTag("Interactable") || miniGameTrigger == null)
            {
                if (hoveredTrigger != null)
                {
                    hoveredTrigger.OnUnhover();
                    hoveredTrigger = null;
                    currentTarget = null;
                    highlight.gameObject.SetActive(false);
                }
                return;
            }
            
            // set highlight position, and enable it, + change the enable/disable if its already done
            if (hoveredTrigger == null || hoveredTrigger != miniGameTrigger)
            {
                hoveredTrigger = miniGameTrigger;
                hoveredTrigger.OnHover();
            }
            
            currentTarget = hit.collider.gameObject;
            BoxCollider boxCollider = currentTarget.GetComponent<BoxCollider>();
            Vector3 screenPos = cam.WorldToScreenPoint(currentTarget.transform.position);
            highlight.position = screenPos;
            highlight.localScale = new Vector3(
                boxCollider.size.x * ScaleRatio.x,
                boxCollider.size.y * ScaleRatio.y,
            1);
            
            highlight.gameObject.SetActive(true);
            highlight.GetChild(0).gameObject.SetActive(!miniGameTrigger.IsActive);

            if (Mouse.current.leftButton.wasPressedThisFrame && miniGameTrigger.IsActive)
            {
                miniGameTrigger.StartGame();
            }
        }
    }
}