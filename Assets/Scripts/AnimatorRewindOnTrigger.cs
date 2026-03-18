using UnityEngine;

public class AnimatorRewindOnTrigger : MonoBehaviour
{
    public Animator animator;
    public string stateName = "AnimationState"; // Name of the animation state

    void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Rewind animation to start
        animator.Play(stateName, 0, 0f);
    }
}