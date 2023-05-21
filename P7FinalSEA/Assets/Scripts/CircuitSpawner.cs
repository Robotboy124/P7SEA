using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitSpawner : MonoBehaviour
{

    public GameObject circuit;
    // Start is called before the first frame update
    void Start()
    {
        CircuitSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CircuitSpawn()
    {
        GameObject objectSpawning = Instantiate(circuit, transform.position + new Vector3 (Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)), Quaternion.identity);
        objectSpawning.GetComponent<Circuit>().spawning = true;
    }
}
