using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentSpawner : MonoBehaviour
{
    public GameObject current;
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
        GameObject objectSpawning = Instantiate(current, transform.position + new Vector3 (Random.Range(-10.0f, 10.0f), Random.Range(-5.0f, 5.0f), Random.Range(-10.0f, 10.0f)), Quaternion.identity);
    }
}
