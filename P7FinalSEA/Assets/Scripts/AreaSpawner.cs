using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSpawner : MonoBehaviour
{
    public GameObject[] enemiesToSpawn;
    public GameObject spawnParticle;
    float spawnCheck;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == GameObject.Find("Player") && spawnCheck == 0)
        {
            for(int i = 0; i < enemiesToSpawn.Length; i++)
            {
                enemiesToSpawn[i].SetActive(true);
                Instantiate(spawnParticle, enemiesToSpawn[i].transform.position, Quaternion.identity);
            }
            if (GetComponent<Blocker>() != null)
            {
                GetComponent<Blocker>().WallSummon();
            }
        }
    }
}
