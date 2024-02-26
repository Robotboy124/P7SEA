using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Current : MonoBehaviour
{
    float speed = 10f;
    public GameObject projectile;
    public GameObject zapPoint;
    public GameObject[] telePoint;
    float telePointTimer = 1.0f;
    int teleport = 0;
    Vector3 movement;
    public Vector3 transformStart;
    public float damageMulti;
    Vector3 teleportStart;
    Damageable damaging;
    GameObject player;
    Rigidbody rb;
    bool raycast = false;
    bool moving = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        transformStart = transform.position;
        damaging = GetComponent<Damageable>();
        player = GameObject.Find("Player");
        StartCoroutine(CurrentAttack());
    }

    // Update is called once per frame
    void Update()
    {
        telePointTimer -= Time.deltaTime;
        if (telePointTimer <= 0)
        {
            teleport++;
            telePointTimer = 1.0f + Random.Range(-0.5f, 0.5f);
        }
        if (teleport % 2 == 0)
        {
            movement = Vector3.left;
        }
        else
        {
            movement = Vector3.right;
        }
        if (moving)
        {
            transform.LookAt (player.transform.position);
            transform.Translate(movement*speed*Time.deltaTime);
        }
        CircuitShoot();
    }

    IEnumerator CurrentAttack()
    {
        yield return new WaitForSeconds(0.75f);
        moving = false;
        yield return new WaitForSeconds(0.75f);
        raycast = true;
    }

    IEnumerator Stopper()
    {
        yield return new WaitForEndOfFrame();
        StopRaycast();
        moving = true;
    }

    void CircuitShoot()
    {
        if (raycast)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, (GameObject.Find("Player").transform.position - transform.position), out hit, Mathf.Infinity))
            {
                GameObject objectSpawning = Instantiate(projectile, zapPoint.transform.position, Quaternion.Euler((new Vector3(90+transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z))));
                objectSpawning.GetComponent<CurrentProj>().Starter();
                objectSpawning.GetComponent<DamageField>().damage = objectSpawning.GetComponent<DamageField>().damage * damageMulti;
                objectSpawning.GetComponent<InstantiatedAttack>().ObjectUpdate(gameObject);
                StartCoroutine(Stopper());
            }
        }
    }

    void StopRaycast()
    {
        raycast = false;
        StartCoroutine(CurrentAttack());
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<ElecWall>() == null)
        {
            return;
        }
        else
        {
            transform.position = transformStart;
            teleport++;
        }
    }
}
