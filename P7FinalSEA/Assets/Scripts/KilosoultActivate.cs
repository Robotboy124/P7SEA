using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KilosoultActivate : MonoBehaviour
{
    public GameObject[] kilosoultObjects;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameObject.Find("Player"))
        {
            other.gameObject.transform.position = Vector3.up*2;
            for (int i = 0; i < kilosoultObjects.Length; i++)
            {
                kilosoultObjects[i].SetActive(true);
            }
        }
    }
}
