using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Stitch : MonoBehaviour
{
    [SerializeField] SpriteRenderer stitchEnd;
    [SerializeField] float snapRadius = 0.2f;   // in the parent's local units

    static readonly List<Stitch> all = new List<Stitch>();

    private Camera cam;
    private Transform anchor;
    private Vector3 homeLocalPos;
    private Quaternion homeLocalRot;
    private Vector3 homeLocalEuler;
    private Vector2 homeSize;
    private bool done;

    void OnEnable()  => all.Add(this);
    void OnDisable() => all.Remove(this);

    void Awake()
    {
        homeSize = stitchEnd.size;
    }
    
    void Start()
    {
        cam = Camera.main;
        anchor = transform.parent;

        homeLocalPos = transform.localPosition;
        homeLocalRot = transform.localRotation;
        homeLocalEuler = transform.localEulerAngles;
    }

    void OnMouseDrag()
    {
        if (done) return;

        // 1. mouse ray -> hit point on the panel plane (the parent's XY plane)
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        Plane plane = new Plane(anchor.forward, anchor.position);
        if (!plane.Raycast(ray, out float enter)) return;

        // 2. world -> 2D coordinates on the panel
        Vector2 target = anchor.InverseTransformPoint(ray.GetPoint(enter));

        // 3. snap check, all in panel 2D space
        foreach (Stitch other in all)
        {
            if (other == this || other.done) continue;

            Vector2 otherPos = anchor.InverseTransformPoint(other.transform.position);
            if (Vector2.Distance(target, otherPos) > snapRadius) continue;

            UpdateWire(otherPos);

            if (transform.parent.name.Equals(other.transform.parent.name))
            {
                Debug.Log("successful stitch");
                StitchController.Instance.StitchDone();
                other.Done();
                Done();
            }
            return;
        }

        UpdateWire(target);
    }

    public void Done()
    {
        done = true;
    }

    public void ResetStitch()
    {
        done = false;
        transform.localPosition = homeLocalPos;
        transform.localRotation = homeLocalRot;
        stitchEnd.size = homeSize;
    }

    private void OnMouseUp()
    {
        if (done) return;
        ResetStitch();
    }

    // target is a 2D point in the parent's local space (wire starts at the parent's origin)
    private void UpdateWire(Vector2 target)
    {
        if (target.sqrMagnitude < 0.0001f) return;

        // only local X, Y position change (Z stays put)
        transform.localPosition = new Vector3(target.x, target.y, homeLocalPos.z);

        // only local Z rotation changes (X and Y stay at their rest values)
        float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(homeLocalEuler.x, homeLocalEuler.y, angle);

        // stretch the sprite to span the distance (measured in world units, like the original)
        float worldLength = Vector3.Distance(transform.position, anchor.position);
        float scaleX = stitchEnd.transform.lossyScale.x;
        stitchEnd.size = new Vector2(worldLength / scaleX, stitchEnd.size.y);
    }
}