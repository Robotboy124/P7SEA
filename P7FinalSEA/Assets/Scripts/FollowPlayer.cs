using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FollowPlayer : MonoBehaviour
{
    public GameObject explodeParticle;
    public float speed;
    public bool spawning;
    Damageable damaging;
    Transform playerPos;
    Vector3 randomPos;
    bool chase;
    // Start is called before the first frame update
    void Start()
    {
        damaging = GetComponent<Damageable>();
        playerPos = GameObject.Find("Player").transform;
        randomPos = new Vector3(Random.Range(-1.25f, 1.25f), Random.Range(-1.25f, 1.25f), Random.Range(-1.25f, 1.25f));
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
<<<<<<< Updated upstream
        if (Physics.Raycast(transform.position, (GameObject.Find("PlayerCam").transform.position - transform.position), out hit, Vector3.Distance(GameObject.Find("PlayerCam").transform.position, transform.position)))
        {
            if (hit.collider.gameObject == playerPos.gameObject)
=======
        if (Physics.Raycast(transform.position, (GameObject.Find("PlayerCam").transform.position-transform.position), out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject == GameObject.Find("Player"))
>>>>>>> Stashed changes
            {
                transform.LookAt(playerPos.position + randomPos);
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
        }
    }

    public void SparkExplode()
    {
        GameObject yeet = Instantiate(explodeParticle, transform.position, Quaternion.identity);
        if (GameObject.Find("Kilosoult").GetComponent<Kilosoult>().soulTimer > 0)
        {
            yeet.GetComponent<DamageField>().playerProj = true;
        }
        Destroy(gameObject);
    }
}
