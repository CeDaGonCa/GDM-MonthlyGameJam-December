using UnityEngine;

public abstract class TriggerEffect : MonoBehaviour
{
    public abstract void OnHover();
    public abstract void OnComplete();
    public abstract void OnUnhover();
}
