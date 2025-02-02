using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NpcManager : MonoBehaviour
{

    private SphereCollider coll;
    public bool evil;
    private bool chasing;
    public float radius = 10f;
    public NpcCharacter[] characters;

    void Awake()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].radius = radius;
        }
        coll.radius = radius;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(evil == true)
        {
            if (other.tag == "Player")
            {
                Debug.Log("time to start chasing");
                chasing = true;
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
                chasing = false;
                for (int i = 0; i < characters.Length; i++)
                {
                    characters[i].StopChase();
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        //Handles.color = Color.cyan;
        //Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, radius);
    }
}
