using UnityEngine;

public class SeesawZone : MonoBehaviour
{
    public SeesawController seesaw;

    [Range(0f, 1f)]
    public float targetTime;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Jammo")) return;

        seesaw.SetSeesawPosition(targetTime);
    }
}