using UnityEngine;
using UnityEngine.InputSystem;

public class Stitch : MonoBehaviour
{
    [SerializeField] SpriteRenderer stitchEnd;
    [SerializeField] float snapRadius = 0.2f;

    private Camera cam;
    private Transform anchor;
    private Vector3 planeNormal;
    private Vector3 homeLocalPos;
    private Quaternion homeLocalRot;
    private Vector2 homeSize;

    void Start()
    {
        Debug.Log("hello what");
        cam = Camera.main;
        anchor = transform.parent;

        // remember the tilted panel's facing direction and the resting pose
        planeNormal = transform.forward;
        homeLocalPos = transform.localPosition;
        homeLocalRot = transform.localRotation;
        homeSize = stitchEnd.size;
    }

    void OnMouseDrag()
    {
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        Plane plane = new Plane(planeNormal, anchor.position);

        if (!plane.Raycast(ray, out float enter)) return;   // was inverted
        Vector3 worldPos = ray.GetPoint(enter);
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(worldPos, snapRadius * transform.lossyScale.x);
        foreach (Collider2D col in colliders)
        {
            if (col.gameObject == gameObject) continue;
            Stitch other = col.GetComponent<Stitch>();
            if (other == null) continue;

            UpdateWire(other.transform.position);

            if (transform.parent.name.Equals(col.transform.parent.name))
            {
                Debug.Log("succesful stitch");
                StitchController.Instance.StitchDone();
                other.Done();
                Done();
            }
            return;
        }

        UpdateWire(worldPos);
    }

    public void Done()
    {
        Destroy(this);
    }

    private void OnMouseUp()
    {
        // reset to the resting pose
        transform.localPosition = homeLocalPos;
        transform.localRotation = homeLocalRot;
        stitchEnd.size = homeSize;
    }

    private void UpdateWire(Vector3 newPos)
    {
        Vector3 start = anchor.position;
        Vector3 direction = Vector3.ProjectOnPlane(newPos - start, planeNormal);
        if (direction.sqrMagnitude < 0.0001f) return;

        transform.position = start + direction;

        // point right along the wire while staying flat on the tilted panel
        transform.rotation = Quaternion.LookRotation(
            planeNormal, Vector3.Cross(planeNormal, direction));

        float scaleX = stitchEnd.transform.lossyScale.x;
        stitchEnd.size = new Vector2(direction.magnitude / scaleX, stitchEnd.size.y);
    }
}