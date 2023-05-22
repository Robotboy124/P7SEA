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
    bool raycast = false;
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
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
    }

    IEnumerator CircuitAttack()
    {
        yield return new WaitForSeconds(2.0f);
        raycast = true;
        RaycastOnce();
    }

    void RaycastOnce()
    {
        if (raycast)
        {
            RaycastHit hit;
            if (Physics.Raycast(zapPoint.position, (player.transform.position - zapPoint.position), out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject == player)
                {
                    GameObject trail = Instantiate(zapTrail, transform.position, Quaternion.identity);
                    trail.GetComponent<ProjectileTrail>().SetPosition(hit.point, zapPoint.position);
                    Instantiate(groundLightning, hit.point, Quaternion.identity);
                    StopRaycast();
                }
                else 
                {
                    StopRaycast();
                }
            }
        }
    }

    void StopRaycast()
    {
        raycast = false;
        StartCoroutine(CircuitAttack());
    }
}
