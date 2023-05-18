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
        if (healthBarr)
        {
            healthBa.UpdateValue(health);
        }
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Damaged(float damage)
    {
        if (player && GameObject.Find("Player").GetComponent<PlayerControls>().parryTimer > 0)
        {
            return;
        }
        else
        {
            health -= damage;
            damageTaken += damage;
        }
    }
}
