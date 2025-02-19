using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    bool triggered;
    public Teleporter target;




    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!triggered)
            {
                other.GetComponent<CharacterController>().enabled = false;
                Debug.Log("teleporter is geactiveerd");
                if (target == null)
                {
                    Debug.LogError("je moet een gameobject toevoegen waar de speler naar getelerporteerd wordt");
                }
                else
                {
                    target.triggered = true;
                    other.transform.position = target.transform.position + new Vector3(0, 0.5f, 0);
                }
                other.GetComponent<CharacterController>().enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
         triggered = false;
    }
}
