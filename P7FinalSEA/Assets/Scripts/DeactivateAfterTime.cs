using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateAfterTime : MonoBehaviour
{

    public float deactivateTime = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        deactivateTime -= Time.deltaTime;

        if (deactivateTime <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
