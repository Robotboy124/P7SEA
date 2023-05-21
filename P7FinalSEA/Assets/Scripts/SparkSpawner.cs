using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkSpawner : MonoBehaviour
{

    public GameObject spark;
    // Start is called before the first frame update
    void Start()
    {
        SparkSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SparkSpawn()
    {
        GameObject objectSpawning = Instantiate(spark, transform.position + new Vector3 (Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)), Quaternion.identity);
        objectSpawning.GetComponent<FollowPlayer>().spawning = true;
    }
}
