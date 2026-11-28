using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class UIAmbientBob : MonoBehaviour
{
    [Header("Bob")]
    [SerializeField] private float distance = 8f;          // total travel is +/- this
    [SerializeField] private Vector2 durationRange = new Vector2(2.5f, 4f);

    [Header("Sway (optional)")]
    [SerializeField] private float rotationAmount = 1.5f;  // degrees, set 0 to disable
    [SerializeField] private Vector2 rotationDurationRange = new Vector2(3.5f, 5.5f);

    [Header("Misc")]
    [SerializeField] private bool ignoreTimeScale = true;  // keeps menus animating if paused

    private RectTransform rect;
    private Vector2 startPos;
    private float startRot;
    private Tween bobTween, swayTween;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        startPos = rect.anchoredPosition;
        startRot = rect.localEulerAngles.z;
    }

    private void OnEnable()
    {
        float bobDur = Random.Range(durationRange.x, durationRange.y);

        rect.anchoredPosition = new Vector2(startPos.x, startPos.y - distance);
        bobTween = rect
            .DOAnchorPosY(startPos.y + distance, bobDur)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetUpdate(ignoreTimeScale)
            .SetLink(gameObject);
        bobTween.Goto(Random.value * bobDur * 2f, true); // random phase

        if (rotationAmount > 0f)
        {
            float rotDur = Random.Range(rotationDurationRange.x, rotationDurationRange.y);

            rect.localEulerAngles = new Vector3(0, 0, startRot - rotationAmount);
            swayTween = rect
                .DOLocalRotate(new Vector3(0, 0, startRot + rotationAmount), rotDur)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo)
                .SetUpdate(ignoreTimeScale)
                .SetLink(gameObject);
            swayTween.Goto(Random.value * rotDur * 2f, true);
        }
    }

    private void OnDisable()
    {
        bobTween?.Kill();
        swayTween?.Kill();
        rect.anchoredPosition = startPos;
        rect.localEulerAngles = new Vector3(0, 0, startRot);
    }
}