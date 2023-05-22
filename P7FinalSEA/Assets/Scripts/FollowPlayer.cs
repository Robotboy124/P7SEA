using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        randomPos = new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f));
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(playerPos.position, transform.position) < 40f)
        {
            transform.LookAt(playerPos.position + randomPos);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
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
