using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;



public class Deadzone : MonoBehaviour
{
    public Vector3 spawnpointpos;
    public PlayerLifeSupport playerlife;
    public PlayerLifeSupport[] playerlifes;
    private bool spawnpointavailable;
    public bool colliderfound;
    public Collider collider;

    private void Start()
    {
        playerlife = FindFirstObjectByType<PlayerLifeSupport>();
        if (collider != null)
        {
            colliderfound = true;
        }
    }

    private bool CheckForSpawnPoints(PlayerLifeSupport lifesupport)
    {
        bool checker = false;
        if (lifesupport.spawnpoints.Length == 0)
            checker = false;
        else
            checker = true;
        return checker;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("je bent dood");
            other.GetComponent<CharacterController>().enabled = false;

            PlayerLifeSupport support = other.GetComponent<PlayerLifeSupport>();
            if (support != null)
            {
                if (CheckForSpawnPoints(support))
                {
                    other.transform.position = playerlife.currspawnpoint;
                }
                else
                {
                    other.transform.position = spawnpointpos + new Vector3(0, 2, 0);
                }
            }
            else
            {
                other.transform.position = spawnpointpos + new Vector3(0, 2, 0);
                Debug.LogError("de speler mist het playerlife script");
            }
            other.GetComponent<CharacterController>().enabled = true;
        }
    }



    private void OnDrawGizmos()
    {
        if(colliderfound == true)
        {
            Gizmos.color = new UnityEngine.Color(1f, 0f, 0f, 0.5f); // Green with custom alpha
            Vector3 gizmoscale = transform.localScale;

            if (collider.GetType() == typeof(BoxCollider))
            {
                gizmoscale = new Vector3(collider.bounds.size.x, collider.bounds.size.y, collider.bounds.size.z);
                Vector3 gizmopos = transform.position - collider.bounds.center;
                Gizmos.DrawCube(collider.bounds.center, gizmoscale);
            }
            if (collider.GetType() == typeof(SphereCollider))
            {
                float gizmoradius = transform.localScale.x * collider.bounds.size.x;
                Gizmos.DrawSphere(collider.bounds.center, collider.bounds.size.y / 2);
            }
            if (collider.GetType() == typeof(CapsuleCollider))
            {
                Gizmos.DrawSphere(collider.bounds.center, collider.bounds.size.x / 2);
                Gizmos.DrawSphere(new Vector3(collider.bounds.size.x, collider.bounds.size.y, collider.bounds.size.z), collider.bounds.size.x / 2);
            }
            if (collider.GetType() == typeof(MeshCollider))
            {
                gizmoscale = new Vector3(transform.localScale.x * collider.bounds.size.x, transform.localScale.y * collider.bounds.size.y, transform.localScale.z * collider.bounds.size.z);
            }
        }
    }
}
