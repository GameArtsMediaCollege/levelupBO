using UnityEngine;

public class SeesawController : MonoBehaviour
{
    public Animator animator;
    public string stateName = "Seesaw";

    public float animationSpeed = 1f;

    private float currentTime = 0.5f;
    private float targetTime = 0.5f;

    void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        animator.speed = 0f;

        currentTime = 0.5f;
        targetTime = 0.5f;

        animator.Play(stateName, 0, currentTime);
        animator.Update(0f);
    }

    void Update()
    {
        if (Mathf.Approximately(currentTime, targetTime))
            return;

        currentTime = Mathf.MoveTowards(currentTime, targetTime, animationSpeed * Time.deltaTime);

        animator.Play(stateName, 0, currentTime);
        animator.Update(0f);
    }

    public void SetSeesawPosition(float newTarget)
    {
        targetTime = Mathf.Clamp01(newTarget);
    }
}