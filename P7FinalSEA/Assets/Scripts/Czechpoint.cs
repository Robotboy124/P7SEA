using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Czechpoint : MonoBehaviour
{
    PlayerControls player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerControls>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == GameObject.Find("Player"))
        {
            player.checkpoint = gameObject.transform;
        }
    }
}
