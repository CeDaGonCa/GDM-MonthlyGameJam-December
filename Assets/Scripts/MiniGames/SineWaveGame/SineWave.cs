
using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(LineRenderer))]
public class SineWave : MonoBehaviour
{
    [SerializeField] int points = 50;
    [SerializeField] float amplitude = 1;
    [SerializeField] float frequency = 1;
    [SerializeField] float frequencyLimit = 2f;
    [SerializeField] float amplitudeLimit = 1f;
    [SerializeField] float lineWidth = 0.05f;
    [SerializeField] float changeAmount = 0.05f;
    [SerializeField] float margins = 2f;

    [SerializeField] Transform device;

    public Action OnSineWaveChanged;
    
    private LineRenderer lineRenderer;
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.alignment = LineAlignment.View;
        lineRenderer.useWorldSpace = false; 
        lineRenderer.widthCurve = AnimationCurve.Constant(0f, 1f, lineWidth * transform.lossyScale.y * 4f);
        RandomizeWave();
        //SineLimits = new Vector2(0,3);
    }
    public void RandomizeWave()
    {
        frequency = Random.Range(1, (int)(frequencyLimit / changeAmount)) * changeAmount;
    }


    void Update()
    {
        Vector3 scale = new Vector3(transform.localScale.x * device.localScale.x, transform.localScale.y * device.localScale.y,
                transform.localScale.z);
        //scale = transform.lossyScale;
        Vector3 offset = new Vector3(0, transform.position.y, transform.position.z - (scale.z / 2));
        //Vector3 offset = transform.position - new Vector3(scale.x/2,0,scale.z/2);
        
        float xStart = transform.position.x - scale.x / 2;
        float xFinish = transform.position.x + scale.x / 2;
        //float xStart = transform.position.x - scale.x / 2 + margins;
        //float xFinish = transform.position.x + scale.x / 2 - margins;
        float TAU = 2 * Mathf.PI;

        lineRenderer.positionCount = points;
        for (int currentPoint = 0; currentPoint < points; currentPoint++)
        {
            float progress = (float)currentPoint / (points - 1);

            //lineRenderer.SetPosition(currentPoint, transform.TransformPoint(local));
            float x = Mathf.Lerp(xStart, xFinish, progress);
            x = progress - 0.5f;
            float y = amplitude * Mathf.Sin((TAU * frequency * x) + Time.timeSinceLevelLoad);
            y = Mathf.Sin(progress * frequency * TAU + Time.timeSinceLevelLoad) * amplitude;
            lineRenderer.SetPosition(currentPoint,new Vector3(x,y,0));
            //lineRenderer.SetPosition(currentPoint, transform.TransformPoint(new Vector3(x, y, 0.01f)));
        }
    }
    public void ChangeValues(bool Adds, bool changeFreq)
    {
        if (changeFreq)
        {
            frequency = Mathf.Clamp(frequency + (Adds ? 1 : -1) * changeAmount, changeAmount, frequencyLimit);
        }
        else
        {
            amplitude = Mathf.Clamp(amplitude + (Adds ? 1 : -1) * changeAmount, changeAmount, frequencyLimit);
        }
        
        OnSineWaveChanged?.Invoke();
    }
    public float Frequency => frequency;
}
