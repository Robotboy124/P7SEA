using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorFollow : MonoBehaviour
{
    public GameObject playerTracking;
    // Start is called before the first frame update
    void Start()
    {
        playerTracking = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerTracking.transform.position - Vector3.up * playerTracking.transform.position.y + Vector3.up * 0.5f;
        //yahoo
    }
}
