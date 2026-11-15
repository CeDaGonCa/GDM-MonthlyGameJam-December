using UnityEngine;
using UnityEngine.InputSystem;

public class PanoramaCamera : MonoBehaviour
{
    public Transform cameraTransform;
    public SpriteRenderer background;

    public float sensitivity = 5f;

    private float minX;
    private float maxX;

    void Start()
    {
        // Calculate bounds from background dimensions
        float backgroundHalfWidth = background.bounds.extents.x;
        float cameraHalfWidth = Camera.main.orthographicSize
                                * Camera.main.aspect;

        minX = background.bounds.min.x + cameraHalfWidth;
        maxX = background.bounds.max.x - cameraHalfWidth;
    }

    void Update()
    {
        float movement = 0f;

        if (Keyboard.current.aKey.isPressed)
            movement = -1f;

        if (Keyboard.current.dKey.isPressed)
            movement = 1f;

        float newX = cameraTransform.position.x
                     + movement * sensitivity * Time.deltaTime;

        newX = Mathf.Clamp(newX, minX, maxX);

        cameraTransform.position = new Vector3(
            newX,
            cameraTransform.position.y,
            cameraTransform.position.z
        );
    }
}