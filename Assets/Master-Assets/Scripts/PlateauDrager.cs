using UnityEngine;
[RequireComponent(typeof(BoxCollider))]
public class PlateauDrager : MonoBehaviour
{
    [SerializeField, HideInInspector]
    BoxCollider collider;
    void Start()
    {
        collider = GetComponent<BoxCollider>();
        collider.isTrigger = true;
    }
    private void OnValidate()
    {
        // Wordt aangeroepen in edit mode wanneer iets verandert in de inspector
        if (collider == null)
        {
            collider = GetComponent<BoxCollider>();
            collider.isTrigger = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Jammo")
        {
            other.transform.SetParent(transform);
            Debug.Log("parent is set");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Jammo")
        {
            other.transform.SetParent(null);
            Debug.Log("parent is let go");
        }
    }


    private void OnDrawGizmos()
    {
        // Set the color with custom alpha.
        Gizmos.color = new UnityEngine.Color(0f, 1f, 0f, 0.5f); // Green with custom alpha

        // Draw the cube.
        Gizmos.DrawCube(transform.position + collider.center, collider.size);
    }
}
