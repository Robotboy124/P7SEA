using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTrail : MonoBehaviour
{

    public Transform player;
    Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPosition(Vector3 hitPos, Vector3 startPos)
    {
        startPosition = startPos;
        StartCoroutine(TrailGenerate(hitPos, startPosition));
    }

    IEnumerator TrailGenerate(Vector3 hit, Vector3 start)
    {
        GetComponent<TrailRenderer>().enabled = false;
        transform.position = start;
        yield return new WaitForEndOfFrame();
        GetComponent<TrailRenderer>().enabled = true;
        yield return new WaitForEndOfFrame();
        transform.position = hit;
    }
}
