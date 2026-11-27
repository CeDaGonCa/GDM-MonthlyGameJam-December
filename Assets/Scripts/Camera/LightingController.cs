using UnityEngine;

public class LightingController : MonoBehaviour
{
    public static LightingController Instance;
    private Animator animator;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        animator = GetComponent<Animator>();
    }
    
    void Start()
    {
        GameController.Instance.GameStarted += OnGameStarted;
    }

    private void OnGameStarted()
    {
        animator.SetTrigger("Start");    
    }
}
