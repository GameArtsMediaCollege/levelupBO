using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(AudioSource))]
public class TriggerAnimation : MonoBehaviour
{
    public List<Animator> animatorslist;
    private AudioSource audiosource;
    private bool triggered; 
    void Start()
    {
        if(animatorslist.Count > 0)
        {
            for (int i = 0; i < animatorslist.Count; i++)
            {
                animatorslist[i].enabled = false;
            }
        }
        audiosource = GetComponent<AudioSource>();
        if(audiosource == null )
        {
            Debug.Log("geen audio gevonden voor de trigger. de trigger zal geen audio afspelen");
        }
    }

    private void SelectInteraction(int i)
    {
        switch (i)
        {
            case 0: //trigger an animation
                Animation();
                break;

        }
    }

    void Animation()
    {
        for (int i = 0; i < animatorslist.Count; i++)
        {
            animatorslist[i].enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (!triggered)
            {
                audiosource.Play();
                SelectInteraction(0);
                triggered = true;
            }
        }
    }
}
