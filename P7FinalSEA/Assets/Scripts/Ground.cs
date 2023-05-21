using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionExit (Collision other)
    {
        if (other.gameObject == GameObject.Find("Player"))
        {
            other.gameObject.GetComponent<PlayerControls>().grounded = false;
        }
    }

    void OnCollisionEnter (Collision other)
    {
        if (other.gameObject == GameObject.Find("Player"))
        {
            other.gameObject.GetComponent<PlayerControls>().grounded = true;
            other.gameObject.GetComponent<PlayerControls>().dashCount = 0;
            if (other.gameObject.GetComponent<PlayerControls>().slamming)
            {
                Instantiate(other.gameObject.GetComponent<PlayerControls>().slamEffect, other.gameObject.transform.position, Quaternion.identity);
                other.gameObject.GetComponent<PlayerControls>().slamming = false;
            }
        }
    }
}
