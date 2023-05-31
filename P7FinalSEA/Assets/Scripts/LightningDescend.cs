using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningDescend : MonoBehaviour
{

    public float yLevel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, yLevel, 0.1f), transform.position.z);
    }
}
