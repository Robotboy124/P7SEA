using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryBlock : MonoBehaviour
{

    Rigidbody rb;
    public GameObject explosion;
    GameObject parryTrail;
    Vector3 targetParry;
    Vector3 targetUpdate;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        parryTrail = GameObject.Find("Parry Trail");
        targetParry = targetUpdate;
        RaycastHit ho;
        GameObject[] grant = GameObject.FindGameObjectsWithTag("Wood");
        GameObject[] sparks = GameObject.FindGameObjectsWithTag("Spark");
        GameObject[] currents = GameObject.FindGameObjectsWithTag("Current");
        GameObject[] circuits = GameObject.FindGameObjectsWithTag("Circuit");
        for (int f = 0; f < grant.Length; f++)
        {
            if (Physics.Raycast(transform.position, (grant[f].transform.position - transform.position), out ho, Mathf.Infinity))
            {
                if (grant[f] != gameObject && grant[f].activeInHierarchy == true)
                {
                    targetUpdate = ho.point;
                }
                else
                {
                    for(int i = 0; i < sparks.Length; i++)
                    {
                        RaycastHit hit;
                        if (Physics.Raycast(transform.position, (sparks[i].transform.position - transform.position), out hit, Mathf.Infinity))
                        {
                            if (hit.collider.gameObject.activeInHierarchy == true)
                            {
                                targetUpdate = hit.point;
                            }
                            else
                            {
                                RaycastHit hi;
                                for (int u = 0; u < currents.Length; u++)
                                {
                                    if (Physics.Raycast(transform.position, (currents[u].transform.position - transform.position), out hi, Mathf.Infinity))
                                    {
                                        if (hit.collider.gameObject.activeInHierarchy == true)
                                        {
                                            targetUpdate = hi.point;
                                        }
                                        else
                                        {
                                            RaycastHit h;
                                            for (int p = 0; p < circuits.Length; p++)
                                            {
                                                if (Physics.Raycast(transform.position, (circuits[p].transform.position - transform.position), out h, Mathf.Infinity))
                                                {
                                                    if (hit.collider.gameObject.activeInHierarchy == true)
                                                    {
                                                        targetUpdate = transform.position;
                                                    }
                                                    else
                                                    {
                                                        Debug.Log("Ignore");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
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
        else if (other.gameObject.GetComponent<ElecWall>() != null)
        {
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("ElecAttack"))
        {
            float damaage = other.gameObject.GetComponent<DamageField>().damage;
            parryTrail.GetComponent<ProjectileTrail>().SetPosition(targetParry, transform.position);
            parryTrail.GetComponent<DamageField>().damage = other.gameObject.GetComponent<DamageField>().damage*1.5f;
            if (other.gameObject.GetComponent<ProjectileTrail>() == null)
            {
                Destroy(other.gameObject);
            }
            Destroy(gameObject);
        }
    }

    public void Explode(float damaging, float radia)
    {
        GameObject objectSpawning = Instantiate(explosion, transform.position, Quaternion.identity);
        objectSpawning.GetComponent<DamageField>().damage = damaging;
        objectSpawning.GetComponent<SphereCollider>().radius = radia;
        Destroy(gameObject);
    }
}
