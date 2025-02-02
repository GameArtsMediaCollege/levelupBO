using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class LevelEndPoint : MonoBehaviour
{
    [HideInInspector] public ParticleSystem particles;
    AudioSource audiosource; 
    void Start()
    {
        audiosource = GetComponent<AudioSource>();
        if(audiosource == null)
        {
            Debug.Log("geen audio voor het eindpunt");
        }
        particles = GetComponentInChildren<ParticleSystem>();
        if (particles == null)
        {
            Debug.Log("geen particles voor het eindpunt gevonden");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if(particles != null) 
            {
                particles.Play();
            }
            Debug.Log("finished level");
        }
    }
}
