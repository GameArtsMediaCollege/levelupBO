using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateauDrager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
