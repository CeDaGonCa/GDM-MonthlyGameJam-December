using Unity.GraphToolkit.Editor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Stitch : MonoBehaviour
{
    [SerializeField] SpriteRenderer stitchEnd;

    private Camera camera;
    Vector3 startPoint;
    Vector3 startPosition;

    void Start()
    {
        camera = Camera.main;
        startPoint = transform.parent.position;
        startPosition = transform.position;
    }
    void OnMouseDrag()
    {
        Vector3 newPos = camera.ScreenToWorldPoint(new Vector3(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue(),0));
        newPos.z = 0;

        // Check for connection points
        Collider2D[] colliders = Physics2D.OverlapCircleAll(newPos,.2f);

        foreach(Collider2D collider in colliders)
        {
            if(collider.gameObject == gameObject) continue;
            UpdateWire(collider.transform.position);
            if (transform.parent.name.Equals(collider.transform.parent.name))
            {
                StitchController.Instance.StitchDone();
                //destroy this script and indicate they are finished
                collider.GetComponent<Stitch>()?.Done();
                Done();
            }
            return;
        }

        // Update Stitch
        UpdateWire(newPos);
    }
    public void Done()
    {
        Destroy(this);
    }
    private void OnMouseUp()
    {
        UpdateWire(startPosition);
    }
    private void UpdateWire(Vector3 newPos)
    {

        transform.position = newPos;

        Vector3 direction = newPos - startPoint;
        transform.right = direction;

        float dist = Vector2.Distance(startPoint, newPos);
        stitchEnd.size = new Vector2(dist, stitchEnd.size.y);
    }
    
}
