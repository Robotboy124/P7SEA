using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitSpawner : MonoBehaviour
{

    public GameObject circuit;
    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn()
    {
        GameObject objectSpawning = Instantiate(circuit, transform.position + new Vector3 (Random.Range(-10.0f, 10.0f), 0, Random.Range(-10.0f, 10.0f)), Quaternion.identity);
        objectSpawning.GetComponent<Circuit>().spawning = true;
    }
}
