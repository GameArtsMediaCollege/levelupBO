using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Deadzone : MonoBehaviour
{
    public Vector3 spawnpointpos;
    public PlayerLifeSupport playerlife;
    public PlayerLifeSupport[] playerlifes;
    private bool spawnpointavailable;

    private void Start()
    {
        playerlife = FindFirstObjectByType<PlayerLifeSupport>();
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
}
