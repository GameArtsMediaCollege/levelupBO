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
        playerlife = FindObjectOfType<PlayerLifeSupport>();
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
        if (other.tag == "Player" && particlespresent)
        {
            playerlife.currspawnpoint = this.transform.position + new Vector3(0, 2,0);
            particles.Play();
            Debug.Log("checkpoint is gezet");
        }
    }
}
