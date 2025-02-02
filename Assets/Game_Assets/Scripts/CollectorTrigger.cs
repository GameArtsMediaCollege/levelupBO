using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider))]
public class CollectorTrigger : MonoBehaviour
{
    public bool BrengSleutelsNaarHetSlot;
    private bool readyforanimation;

    private Animator animator;
    private Collider col;
    public List<SleutelCollectible> sleutel_collectibles;
    private SleutelCollectible[] sleutel_collectibles_full;

    void Awake()
    {
        col = GetComponent<Collider>();
        if (col == null && BrengSleutelsNaarHetSlot == true)
        {
            Debug.LogError("als je de deur alleen wilt openen wanneer de sleutel naar de poort gebracht wordt, dan moet je een collider toevoegen aan dit object");
        }
        else if (BrengSleutelsNaarHetSlot && col.isTrigger == false)
        {
            Debug.LogError("zorg ervoor dat de collider van dit object wordt gebruikt als een trigger door op de knop 'isTrigger' te drukken");
        }
        animator = this.GetComponent<Animator>();
        if(animator == null)
        {
            Debug.LogError("je hebt een animator component nodig om dit slotscript te laten werken. je moet dit component op dit object toeveogen om te werken");
        }
        else
        {
            animator.enabled = false;
        }
        if(sleutel_collectibles.Count == 0)
        {
            Debug.LogError("je hebt geen sleutels toegevoegd aan je lijst. het slot zal niet werken zonder een sleutel");
        }
        else
        {
            sleutel_collectibles_full = new SleutelCollectible[sleutel_collectibles.Count];
            for (int i = 0; i < sleutel_collectibles.Count; i++)
            {
                sleutel_collectibles[i].slotscript = this;
                sleutel_collectibles_full[i] = sleutel_collectibles[i];
            }
        }
    }

    public void Collected(SleutelCollectible sleutel)
    {
        for (int i = 0; i < sleutel_collectibles.Count; i++)
        {
            if(sleutel == sleutel_collectibles[i])
            {
                Debug.Log("sleutel nummer" + sleutel_collectibles[i] + "is gevonden");
                sleutel_collectibles.Remove(sleutel_collectibles[i]);
                CheckSlotList();
            }
        }
    }

    private void CheckSlotList()
    {
        if(sleutel_collectibles.Count == 0)
        {
            readyforanimation = true;
            if (!BrengSleutelsNaarHetSlot)
            {
                animator.enabled = true;
                DestroyKeys();
                Debug.Log("slot is geopend");
            }
        }
        else
        {
            Debug.Log("nog " + sleutel_collectibles.Count + " te verzamelen om het slot te openen");
        }
    }

    private void DestroyKeys()
    {
        for (int i = 0; i < sleutel_collectibles_full.Length; i++)
        {
            sleutel_collectibles_full[i].unlocked = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (BrengSleutelsNaarHetSlot && readyforanimation)
        {
            if(other.tag == "Player")
            {
                DestroyKeys();
                animator.enabled = true;
                Debug.Log("slot is geopend");
            }
        }
    }
}
