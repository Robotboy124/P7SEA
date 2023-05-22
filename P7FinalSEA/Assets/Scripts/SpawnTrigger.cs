using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    public GameObject[] spawners;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameObject.Find("Player"))
        {
            for(int i = 0; i < spawners.Length; i++)
            {
                if (spawners[i] == GameObject.Find("Spark Spawner"))
                {
                    spawners[i].GetComponent<SparkSpawner>().Spawn();
                }
                else if(spawners[i] == GameObject.Find("Circuit Spawner"))
                {
                    spawners[i].GetComponent<CircuitSpawner>().Spawn();
                }
                else if (spawners[i] == GameObject.Find("Current Spawner"))
                {
                    spawners[i].GetComponent<CurrentSpawner>().Spawn();
                }
            }
        }
    }
}
