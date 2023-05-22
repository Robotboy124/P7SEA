using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Damageable : MonoBehaviour
{
    public float health = 100;
    public float initialHealth;
    public GameObject healths;
    public GameObject deathText;
    int deathCount = 0;
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
            deathCount += 1;
            GameObject.Find("Player").GetComponent<PlayerControls>().Respawn();
            if (deathText == null && !player)
            {
                return;
            }
            else if (deathText != null && player)
            {
                deathText.GetComponent<TMP_Text>().text = "Deaths: " + deathCount;
            }
            if (GameObject.Find("Kilosoult") == null)
            {
                return;
            }
            else
            {
                GameObject.Find("Kilosoult").GetComponent<Damageable>().health = GameObject.Find("Kilosoult").GetComponent<Damageable>().initialHealth;
            }
        }
    }

    public void Damaged(float damage)
    {
        health -= damage;
        if (damage > 0)
        {
            damageTaken += damage;
        }
    }
}
