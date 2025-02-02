using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeSupport : MonoBehaviour
{

    [HideInInspector] public LevelSpawnPoint[] spawnpoints;
    [HideInInspector] public Vector3 currspawnpoint;
    [HideInInspector] public int livecounter;

    void Start()
    {
        CheckforSpawn();
    }

    private void CheckforSpawn()
    {
        spawnpoints = FindObjectsOfType<LevelSpawnPoint>();
        if(spawnpoints.Length > 0 )
        {
            currspawnpoint = spawnpoints[0].transform.position + new Vector3(0, 2, 0); 
        }
    }

}
