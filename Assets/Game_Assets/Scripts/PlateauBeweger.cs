using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlateauBeweger : MonoBehaviour
{
    [SerializeField] public Vector3 stablescale;
    [SerializeField] public Vector3 position1;
    [SerializeField] public Vector3 position2;
    [SerializeField] public float speed;
    [HideInInspector] public GameObject plateau;

    private bool switching;
    private Vector3 target;

    void FixedUpdate()
    {
        if(switching == false)
        {
            target = transform.position + position1;
        }
        else if(switching == true)
        {
            target = transform.position + position2;
        }


        if(plateau.transform.position == transform.position + position1)
        {
            switching = true;
        }
        else if (plateau.transform.position == transform.position + position2)
        {
            switching = false;
        }
        plateau.transform.position = Vector3.MoveTowards(plateau.transform.position, target, speed * Time.deltaTime);
    }
}
