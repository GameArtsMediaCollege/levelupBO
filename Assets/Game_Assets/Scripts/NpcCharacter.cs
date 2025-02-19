using System.Collections;
using UnityEngine;

public class NpcCharacter : MonoBehaviour
{
    private IEnumerator routine;


    [HideInInspector]
    public Vector3 baseposition;
    [HideInInspector]
    public float radius;
    public float loopsnelheid;
    public float acceleration;
    void Start()
    {
        baseposition = transform.position;
        selectmovement();
    }

    void selectmovement()
    {
        if(routine != null)
        {
            StopCoroutine(routine);
        }
        Debug.Log("selecteer nieuwe optie");
        int randchannel = Random.Range(0,2);
        switch (randchannel)
        {
                case 0: //stand idle
                routine = idleroutine();

                break;

                case 1: //move to selected area
                routine = walkroutine();
                break;
        }
        StartCoroutine(routine);
    }

    private IEnumerator idleroutine()
    {
        Debug.Log("doe niks");
        yield return new WaitForSeconds(Random.Range(3,8));
        selectmovement();
    }

    private IEnumerator walkroutine()
    {
        Debug.Log("begint met lopen");
        Vector3 target = findtarget();
        Debug.Log("target");
        float curspeed = 0;
        Vector3 targetDirection = target - transform.position;
        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            float step = loopsnelheid * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, step * 2, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);

            curspeed += acceleration * Time.deltaTime;
            if (curspeed > loopsnelheid)
                curspeed = loopsnelheid;

            transform.position = Vector3.MoveTowards(transform.position, target, curspeed * Time.deltaTime);
            yield return null;
        }
        selectmovement();
    }

    private Vector3 findtarget()
    {
        Vector3 temppos = baseposition + Random.insideUnitSphere * radius;
        return new Vector3(temppos.x, baseposition.y, temppos.z);
    }

    public void evilchase(Transform target)
    {
        if(routine != null)
        {
            StopCoroutine(routine);
        }
        routine = chaseroutine(target);
        StartCoroutine(routine);
    }

    public void StopChase()
    {
        if(routine != null)
        {
            StopCoroutine(routine);
        }
        selectmovement();
    }

    private IEnumerator chaseroutine(Transform target)
    {
        float curspeed = 0;
        Vector3 targetpos = new Vector3(target.position.x, baseposition.y, target.position.z);
        while (true)
        {
            while(Vector3.Distance(transform.position, targetpos) > 1f)
            {
                targetpos = new Vector3(target.position.x, baseposition.y, target.position.z);
                Vector3 targetDirection = target.position - transform.position;

                float step = loopsnelheid * Time.deltaTime;

                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, step * 2, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDirection);

                curspeed += acceleration * Time.deltaTime;
                if (curspeed > loopsnelheid)
                    curspeed = loopsnelheid;

                transform.position = Vector3.MoveTowards(transform.position, targetpos, curspeed * Time.deltaTime);
                yield return null;
            }

            yield return null;
        }
    }
}
