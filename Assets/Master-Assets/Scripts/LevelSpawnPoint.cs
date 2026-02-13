using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawnPoint : MonoBehaviour
{
    PlayerLifeSupport playerlife;
    [HideInInspector] public ParticleSystem particles;
    private bool particlespresent;
    void Start()
    {
        playerlife = FindFirstObjectByType<PlayerLifeSupport>();
        particles = GetComponentInChildren<ParticleSystem>();  
        if(particles == null)
        {
            particlespresent = false;
        }
        else
        {
            particlespresent= true; 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Jammo")
        {
            playerlife.currspawnpoint = this.transform.position + new Vector3(0, 2,0);
            if (particlespresent)
            {
                particles.Play();
            }
                Debug.Log("checkpoint is gezet");
        }
    }
}
