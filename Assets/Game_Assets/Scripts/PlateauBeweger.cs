using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlateauBeweger : MonoBehaviour
{
    [SerializeField] public Vector3 stablescale;
    [SerializeField] public Vector3 position1;
    [SerializeField] public Vector3 position2;
    [SerializeField] public float timeToReach;
    [SerializeField] public float hold;
    [HideInInspector] public GameObject plateau;
    private IEnumerator routine; 
    private bool switching;
    private Vector3 target;

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
