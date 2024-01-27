using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticScripting : MonoBehaviour
{
    public DamageField damage;
    float damageInit;
    public PlayerControls player;
    // Start is called before the first frame update
    void Start()
    {
        damageInit = damage.damage;
    }

    // Update is called once per frame
    void Update()
    {
        float burst = player.burstFire;
        float maxAutoBurn = player.maxAutoBurnOut;
        damage.damage = damageInit * (1f - (burst/maxAutoBurn)) * Mathf.Pow(player.burstCount+1, 0.4f);
        Debug.Log(damageInit);
        Debug.Log(damage.damage);
    }
}
