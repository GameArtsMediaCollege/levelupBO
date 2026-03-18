using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(AudioSource))]

public class TriggerSpecialAnimation : MonoBehaviour
{
    public List<Animator> animatorslist;
    public string animatorBoolean;
    private AudioSource audiosource;
    private bool triggered;
    void Start()
    {
        if (animatorslist.Count > 0)
        {

        }
        else
        {
            Debug.Log("geen animators toegevoegd aan dit object. er zal niks gebeuren");
        }
        audiosource = GetComponent<AudioSource>();
        if (audiosource == null)
        {
            Debug.Log("geen audio gevonden voor de trigger. de trigger zal geen audio afspelen");
        }
        if (animatorBoolean == "")
        {
            Debug.Log("er is geen boolean aangetroffen in de animator controller");
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
            animatorslist[i].SetBool(animatorBoolean, true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Jammo")
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