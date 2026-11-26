using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CameraRaycast : MonoBehaviour
{
    public float interactionDistance = 10f;
    public RectTransform highlight;

    private Camera cam;
    private GameObject currentTarget;

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
            // hitting an interactable body part here
            if (hit.collider.CompareTag("Interactable"))
            {
                // return early, no point doing anything else if Trigger is null
                MiniGameTrigger miniGameTrigger = hit.collider.GetComponent<MiniGameTrigger>();
                if (miniGameTrigger == null)
                {
                    Debug.Log("Something is wrong with triggers");
                    return;
                }
                
                // set highlight position, and enable it, + change the enable/disable if its already done
                currentTarget = hit.collider.gameObject;
                Vector3 screenPos = cam.WorldToScreenPoint(currentTarget.transform.position);
                highlight.position = screenPos;
                
                highlight.gameObject.SetActive(true);
                highlight.GetChild(0).gameObject.SetActive(!miniGameTrigger.IsActive);

                if (Mouse.current.leftButton.wasPressedThisFrame && miniGameTrigger.IsActive)
                {
                    miniGameTrigger.StartGame();
                }

                return;
            }
        }

        currentTarget = null;
        highlight.gameObject.SetActive(false);
    }
}