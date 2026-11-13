using UnityEngine;
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

    void Update()
    {
        Ray ray = cam.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                currentTarget = hit.collider.gameObject;

                // Project hitbox centre into screen coordinates
                Vector3 screenPos = cam.WorldToScreenPoint(
                    currentTarget.transform.position
                );

                highlight.position = screenPos;
                highlight.gameObject.SetActive(true);
                return;
            }
        }

        currentTarget = null;
        highlight.gameObject.SetActive(false);
    }
}