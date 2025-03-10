using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider))]
public class PlateauDrager : MonoBehaviour
{
    Collider collider;
    void Start()
    {
        collider = GetComponent<Collider>();
        collider.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.SetParent(transform);
        Debug.Log("parent is set");
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
        Debug.Log("parent is let go");
    }
}
