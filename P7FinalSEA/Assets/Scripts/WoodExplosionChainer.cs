using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodExplosionChainer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Parry"))
        {
            other.gameObject.GetComponent<ParryBlock>().Explode(GetComponent<DamageField>().damage, GetComponent<SphereCollider>().radius);
        }
    }
}
