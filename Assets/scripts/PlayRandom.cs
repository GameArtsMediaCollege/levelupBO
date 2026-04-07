using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandom : MonoBehaviour
{
    private Animator anim;

    [Header("Vul hieronder de exacte naam van je animatie in")]
    [SerializeField] private string animationName = "ExacteNaamAnimatie"; // Choose animation in Inspector

    void Start()
    {
        anim = GetComponent<Animator>();
        if (anim != null && !string.IsNullOrEmpty(animationName))
        {
            anim.Play(animationName, -1, Random.Range(0.0f, 1.0f));
        }
        else
        {
            Debug.LogWarning("Animator component or animation name is missing!");
        }
    }
}