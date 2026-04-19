using UnityEngine;

public class SimplePortal : MonoBehaviour
{
    public Transform exitPortal;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Jammo"))
        {
            other.transform.position = exitPortal.position + Vector3.forward * 2f;
        }
    }
}