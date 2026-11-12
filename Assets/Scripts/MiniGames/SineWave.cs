using UnityEngine;
using UnityEngine.iOS;

[RequireComponent(typeof(LineRenderer))]
public class SineWave : MonoBehaviour
{
    [SerializeField] int points = 50;
    [SerializeField] float amplitude = 1;
    [SerializeField] float frequency = 1;
    [SerializeField] float frequencyLimit = 2f;
    [SerializeField] float lineWidth = 0.05f;
    [SerializeField] float changeAmount = 0.05f;

    [SerializeField] Transform device;
    private LineRenderer lineRenderer;
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.alignment = LineAlignment.View;
        lineRenderer.useWorldSpace = true;
        frequency = Random.Range(1,(int)frequencyLimit / changeAmount) * changeAmount;
        //SineLimits = new Vector2(0,3);
    }

    void Update()
    {
        Vector3 scale = new Vector3(transform.localScale.x * device.localScale.x, transform.localScale.y * device.localScale.y, 
                transform.localScale.z * device.localScale.z);
        Vector3 offset = new Vector3(0,transform.position.y,transform.position.z - (scale.z / 2));
        //Vector3 offset = transform.position - new Vector3(scale.x/2,0,scale.z/2);
        lineRenderer.widthCurve = AnimationCurve.Constant(0f,1f,lineWidth);
        float xStart = transform.position.x - scale.x/2;
        float xFinish = transform.position.x + scale.x/2;
        float TAU = 2 * Mathf.PI;

        lineRenderer.positionCount = points;
        for(int currentPoint = 0; currentPoint < points; currentPoint++)
        {
            float progress = (float) currentPoint / (points - 1);
            float x = Mathf.Lerp(xStart,xFinish, progress);
            float y = amplitude * Mathf.Sin((TAU * frequency * x) + Time.timeSinceLevelLoad);
            lineRenderer.SetPosition(currentPoint, new Vector3(x,y,0) + offset);
        }
    }
    public void ChangeFrequency(bool Adds)
    {
        frequency = Mathf.Clamp(frequency + (Adds? 1 : -1 ) * changeAmount,changeAmount, frequencyLimit);
    }
    public float Frequency => frequency;
}
