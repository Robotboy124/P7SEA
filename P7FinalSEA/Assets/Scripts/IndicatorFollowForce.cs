using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorFollowForce : MonoBehaviour
{
    public GameObject playerTracking;
    Rigidbody rb;
    public float force;
    // Start is called before the first frame update
    void Start()
    {
        playerTracking = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerTrans = playerTracking.transform.position;
        rb.AddForce((playerTrans - transform.position).normalized * force);
        //yahoo
    }
}
