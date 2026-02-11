using UnityEngine;

public class JammoSettings : MonoBehaviour
{
    PlayerStateMachine playerStateMachine;
    [Header("beweging")]
    [Range(1.0f, 10.0f)]
    [SerializeField] private float walkMultiplier = 3.0f;
    [Range(1.0f, 10.0f)]
    [SerializeField] private float runMultiplier = 5.0f;
    [Header("sprong")]
    [Range(0f, 100f)]
    [SerializeField] private float fallMultiplier = 2.0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
