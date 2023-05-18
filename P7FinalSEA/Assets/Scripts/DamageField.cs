using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageField : MonoBehaviour
{

    public float damage;
    public bool eleCannon;
    public bool playerProj;
    public bool automatic;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (eleCannon)
        {
            damage = GameObject.Find("Player").GetComponent<Damageable>().damageTaken * 2f;
        }
    }


    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject == GameObject.Find("Player") && !playerProj)
        {
            collision.gameObject.GetComponent<Damageable>().Damaged(damage);
        }
    }
}
