using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExclusiveSlamScript : MonoBehaviour
{
    public float damage;
    public GameObject slamEffect;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(slamEffect, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject != GameObject.Find("Player"))
        {
            collision.gameObject.GetComponent<Damageable>().Damaged(damage);
        }
    }
    public void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject != GameObject.Find("Player"))
        {
            collision.gameObject.GetComponent<Damageable>().Damaged(damage);
        }
    }
}
