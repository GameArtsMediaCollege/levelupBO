using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;


[RequireComponent(typeof(Collider))]

public class SleutelCollectible : MonoBehaviour
{
    [HideInInspector]
    public SlotScript slotscript;
    private Collider col;
    private float smoothTime = 0.8f;
    private float distanceThreshold = 1;
    private Vector3 velocity = Vector3.zero;
    private float offset = -1;



    [HideInInspector]
    public bool unlocked = false;
    private bool connected = false;
    private float amplitude = 0.2f;
    private float frequency = 0f;
    private float speed = 40;
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    private void Awake()
    {
        if (col != null)
        {
            col.isTrigger = true;
        }
    }

    void Start()
    {
        posOffset = transform.position;
    }


    void FixedUpdate()
    {
        if (!connected)
        {
            transform.Rotate(new Vector3(1, 0, 1) * speed * Time.deltaTime);
            floating();
        }
    }

    private void floating()
    {
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
        transform.position = tempPos;
    }

    private IEnumerator followplayer(Transform player)
    {
        slotscript.Collected(this);
        connected = true;
        while (unlocked == false)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if(distance > distanceThreshold)
            {
                Vector3 direction = player.TransformDirection(Vector3.forward) * offset;
                Vector3 playerposition = new Vector3(player.position.x, player.position.y + 1, player.position.z) + direction;  

                transform.position = Vector3.SmoothDamp(transform.position, playerposition, ref velocity, smoothTime);
            }
            yield return null;
        }

        Vector3 currentsize = transform.localScale;
        float timeElapsed = 0;
        float lerpDuration = 0.5f;
        while (timeElapsed < lerpDuration)
        {
            transform.localScale = Vector3.Lerp(currentsize, Vector3.zero, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Jammo")
        {
            if(slotscript != null)
            {
                StartCoroutine(followplayer(other.transform));
            }
            else
            {
                Debug.LogError("dit sleutelobject is niet gekoppeld aan een slotscript");
            }
        }
    }
}
