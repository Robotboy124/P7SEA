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
    GameObject[] grant;
    GameObject[] sparks;
    GameObject[] currents;
    GameObject[] circuits;
    float woodChecker;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        parryTrail = GameObject.Find("Parry Trail");
    }

    // Update is called once per frame
    void Update()
    {
        targetParry = targetUpdate;
        grant = GameObject.FindGameObjectsWithTag("Wood");
        sparks = GameObject.FindGameObjectsWithTag("Spark");
        currents = GameObject.FindGameObjectsWithTag("Current");
        circuits = GameObject.FindGameObjectsWithTag("Circuit");
        RaycastHit ho;
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
        else if (other.gameObject.CompareTag("ElecAttack") || other.gameObject.CompareTag("Elecannon"))
        {
            float damaage = other.gameObject.GetComponent<DamageField>().damage;
            GameObject[] woods = GameObject.FindGameObjectsWithTag("Parry");
            for(int z = 0; z < woods.Length; z++)
            {
                if(woods[z] == gameObject)
                {
                    woodChecker++;
                }
                else
                {
                    parryTrail.GetComponent<ProjectileTrail>().SetPosition(woods[z].transform.position, transform.position);
                    z = woods.Length+1;
                    woodChecker = 0;
                }
                if (woods.Length < 2)
                {
                    if (other.gameObject.GetComponent<InstantiatedAttack>() == null)
                    {
                        if (sparks.Length > 0)
                        {
                            parryTrail.GetComponent<ProjectileTrail>().SetPosition(sparks[0].transform.position, transform.position);
                            Debug.Log("Aimed for Spark");
                        }
                        else if (currents.Length > 0)
                        {
                            parryTrail.GetComponent<ProjectileTrail>().SetPosition(currents[0].transform.position, transform.position);
                            Debug.Log("Aimed for Current");
                        }
                        else if (circuits.Length > 0)
                        {
                            parryTrail.GetComponent<ProjectileTrail>().SetPosition(circuits[0].transform.position, transform.position);
                            Debug.Log("Aimed for Circuit");
                        }
                        else if (GameObject.Find("Kilosoult") != null)
                        {
                            parryTrail.GetComponent<ProjectileTrail>().SetPosition(GameObject.Find("Kilosoult").transform.position, transform.position);
                        }
                    }
                    else
                    {
                        if (gameObject.CompareTag("Circuit"))
                        {
                            parryTrail.GetComponent<ProjectileTrail>().SetPosition(other.gameObject.GetComponent<InstantiatedAttack>().objectSpawnedThis.transform.position + Vector3.down*0.75f, transform.position);
                        }
                        else
                        {
                            parryTrail.GetComponent<ProjectileTrail>().SetPosition(other.gameObject.GetComponent<InstantiatedAttack>().objectSpawnedThis.transform.position, transform.position);
                        }
                    }
                }
                else if (woodChecker >= woods.Length)
                {
                    parryTrail.GetComponent<ProjectileTrail>().SetPosition(other.gameObject.GetComponent<InstantiatedAttack>().objectSpawnedThis.transform.position, transform.position);
                }
            }
            parryTrail.GetComponent<DamageField>().damage = damaage*1.5f;
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
