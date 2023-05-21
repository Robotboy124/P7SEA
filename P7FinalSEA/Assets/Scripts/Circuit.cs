using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circuit : MonoBehaviour
{
    GameObject player;
    Damageable damaging;
    public Transform zapPoint;
    public GameObject zapTrail;
    public GameObject groundLightning;
    public bool spawning;
    // Start is called before the first frame update
    void Start()
    {
        damaging = GetComponent<Damageable>();
        player = GameObject.Find("Player");
        StartCoroutine(CircuitAttack());
    }

    // Update is called once per frame
    void Update()
    {
        if (damaging.health <= 0 && spawning)
        {
            GameObject.Find("Circuit Spawner").GetComponent<CircuitSpawner>().CircuitSpawn();
        }
    }

    IEnumerator CircuitAttack()
    {
        RaycastHit hit;

        if (Physics.Raycast(zapPoint.position, (player.transform.position - zapPoint.position), out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject != player)
            {
                StartCoroutine(CircuitAttack());
            }
            else
            {
                GameObject trail = Instantiate(zapTrail, transform.position, Quaternion.identity);
                trail.GetComponent<ProjectileTrail>().SetPosition(hit.point, zapPoint.position);
                Instantiate(groundLightning, hit.point, Quaternion.identity);
                yield return new WaitForSeconds(2.0f);
            }
        }
        StartCoroutine(CircuitAttack());
    }
}
