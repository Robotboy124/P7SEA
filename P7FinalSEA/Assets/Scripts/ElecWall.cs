using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElecWall : MonoBehaviour
{
    public float damage;
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
        if(other.gameObject == GameObject.Find("Player"))
        {
            other.gameObject.transform.position = new Vector3 (0, 1, 0);
            other.gameObject.GetComponent<Damageable>().Damaged(damage);
        }
    }
}
