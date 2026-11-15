using UnityEngine;
using UnityEngine.InputSystem;

public class FreeLookCamera : MonoBehaviour
{
    public float sensitivity = 0.15f;
    
    public float minYaw = -80f;
    public float maxYaw = 80f;
    public float minPitch = -80f;
    public float maxPitch = 80f;

    private float yaw;
    private float pitch;
    private CameraRaycast cameraRaycast;
    private void Awake()
    {
        cameraRaycast = GetComponent<CameraRaycast>();
    }
    public void HandleStart()
    {
        Vector3 rotation = transform.eulerAngles;
        yaw = rotation.y;
        pitch = rotation.x;

        Cursor.lockState = CursorLockMode.Locked;
    }

    public void HandleUpdate()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        yaw += mouseDelta.x * sensitivity;
        pitch -= mouseDelta.y * sensitivity;

        yaw = Mathf.Clamp(yaw, minYaw, maxYaw);
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        cameraRaycast.HandleUpdate();
    }
}