using UnityEngine;

public class SeesawReset : MonoBehaviour
{
    public Animator animator;
    public string stateName = "Seesaw";

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (animator != null)
        {
            animator.speed = 0f;
            animator.Play(stateName, 0, 0.5f); // start neutral
            animator.Update(0f);
        }
    }

    public void SetSeesawPosition(float normalizedTime)
    {
        if (animator == null) return;

        animator.speed = 0f;
        animator.Play(stateName, 0, normalizedTime);
        animator.Update(0f);
    }
}