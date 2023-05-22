using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOnDeath : MonoBehaviour
{
    public float distanceForHeal;
    Damageable damaging;
    // Start is called before the first frame update
    void Start()
    {
        damaging = GetComponent<Damageable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (damaging.health <= 0 && Vector3.Distance(GameObject.Find("Player").transform.position, transform.position) < distanceForHeal)
        {
            GameObject.Find("Player").GetComponent<Damageable>().Damaged(damaging.initialHealth * -1f);
        }
    }
}
