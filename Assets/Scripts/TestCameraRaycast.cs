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
            // hitting an interactable body part here
            if (hit.collider.CompareTag("Interactable"))
            {
                currentTarget = hit.collider.gameObject;
                Vector3 screenPos = cam.WorldToScreenPoint(
                    currentTarget.transform.position
                );

                highlight.position = screenPos;
                
                // currently all its doing is setting the UI piece to be true, so probably add something here to do something when the raycast is detecting a thing
                // i.e. "If i press a button in this state where i am hovering over smth interactable, start the minigame processs"
                // alternatively u can move all this logic out of Update and just have a "OnHighlight" and "OnUnhighlight" event, but meh its prolly fine as is 
                highlight.gameObject.SetActive(true);
                return;
            }
        }

        currentTarget = null;
        highlight.gameObject.SetActive(false);
    }
}