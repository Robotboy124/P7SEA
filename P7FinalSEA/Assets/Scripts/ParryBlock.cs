using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryBlock : MonoBehaviour
{

    Rigidbody rb;
    public GameObject explosion;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Spark"))
        {
            other.gameObject.GetComponent<FollowPlayer>().SparkExplode();
        }
        else if (other.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }

    public void Explode(float damage, float radius)
    {
        GameObject objectSpawning = Instantiate(explosion, transform.position, Quaternion.identity);
        objectSpawning.GetComponent<DamageField>().damage = damage;
        objectSpawning.GetComponent<SphereCollider>().radius = radius;
        Destroy(gameObject);
    }
}
