using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{

    public GameObject target;




    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<CharacterController>().enabled = false;
            Debug.Log("teleporter is geactiveerd");
            if(target == null)
            {
                Debug.LogError("je moet een gameobject toevoegen waar de speler naar getelerporteerd wordt");
            }
            else
            {
                other.transform.position = target.transform.position + new Vector3(0, 2, 0);
            }
            other.GetComponent<CharacterController>().enabled = true;
        }
    }
}
