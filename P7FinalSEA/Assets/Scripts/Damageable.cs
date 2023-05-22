using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public float health = 100;
    public float initialHealth;
    public GameObject healths;
    public bool healthBarr;
    public bool player;
    public float damageTaken;
    public float healDistance;
    HealthBar healthBa;
    // Start is called before the first frame update
    void Start()
    {
        initialHealth = health;
        if (healthBarr)
        {
            healthBa = healths.GetComponent<HealthBar>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (health >= initialHealth)
        {
            health = initialHealth;
        }
        if (healthBarr)
        {
            healthBa.UpdateValue(health);
        }
        if (health <= 0 && !player)
        {
            if (Vector3.Distance(GameObject.Find("Player").transform.position, transform.position) < healDistance)
            {
                GameObject.Find("Player").GetComponent<Damageable>().Damaged(-initialHealth);
            }
            Destroy(gameObject);
        }
        else if (health <= 0 && player)
        {
            GameObject.Find("Player").GetComponent<PlayerControls>().Respawn();
        }
    }

    public void Damaged(float damage)
    {
        health -= damage;
        damageTaken += damage;
    }
}
