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

    private void Start()
    {
        position1 = transform.position;
    }

    void FixedUpdate()
    {
        if(switching == false)
        {
            target = position1;
        }
        else if(switching == true)
        {
            target = position2;
        }


        if(transform.position == position1)
        {
            switching = true;
        }
        else if (transform.position == position2)
        {
            switching = false;
        }
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }
}
