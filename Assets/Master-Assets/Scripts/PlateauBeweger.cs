using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlateauBeweger : MonoBehaviour
{
    [SerializeField] private BoxCollider collider;
    [SerializeField] public Vector3 stablescale;
    [SerializeField] public Vector3 position1;
    [SerializeField] public Vector3 position2;
    [SerializeField] public float timeToReach;
    [SerializeField] public float hold;
    [HideInInspector] public GameObject plateau;
    private IEnumerator routine; 
    private bool switching;
    private Vector3 target;

    private Rigidbody rb;
    private float timer;
    private bool toPos2 = true;
    private float holdTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    private void Start()
    {
        position1 = transform.position;
        RouteChecker();
    }
    void RouteChecker()
    {
        if(switching == true)
        {
            switching = false;
            target = position1;
        }
        else
        {
            switching = true;
            target = position2;
        }

        if(routine != null)
        {
            StopCoroutine(routine);
        }
        routine = GoTowardsTarget(target);
        StartCoroutine(routine);
    }

    private void FixedUpdate()
    {
        /*if (holdTimer > 0f)
        {
            holdTimer -= Time.fixedDeltaTime;
            return;
        }

        timer += Time.fixedDeltaTime;
        float t = Mathf.Clamp01(timer / timeToReach);
        t = t * t * (3f - 2f * t); // smoothstep

        Vector3 from = toPos2 ? position1 : position2;
        Vector3 to = toPos2 ? position2 : position1;

        Vector3 next = Vector3.Lerp(from, to, t);
        rb.MovePosition(next);

        if (timer >= timeToReach)
        {
            timer = 0f;
            toPos2 = !toPos2;
            holdTimer = hold;
        }*/
    }

    private IEnumerator GoTowardsTarget(Vector3 target)
    {
        float timeElapsed = 0;
        Vector3 startpos = transform.position;
        while (timeElapsed < timeToReach)
        {
            float t = timeElapsed / timeToReach;
            t = t * t * (3f - 2f * t);
            transform.position = Vector3.Lerp(startpos, target, t);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        yield return new WaitForSeconds(hold);
        RouteChecker();
    }
}
