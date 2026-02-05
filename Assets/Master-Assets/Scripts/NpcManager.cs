using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class NpcManager : MonoBehaviour
{

    private SphereCollider coll;
    public bool evil;
    public NpcCharacter[] characters;

    void Start()
    {
        coll = GetComponent<SphereCollider>();
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].radius = coll.radius;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(evil == true)
        {
            if (other.tag == "Player")
            {
                Debug.Log("time to start chasing");
                for (int i = 0; i < characters.Length; i++)
                {
                    characters[i].evilchase(other.transform);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (evil == true)
        {
            if (other.tag == "Player")
            {
                Debug.Log("time to stop chasing");
                for (int i = 0; i < characters.Length; i++)
                {
                    characters[i].StopChase();
                }
            }
        }
    }
}
