using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueLightningMovement : MonoBehaviour
{
    Vector3 localPosInit;
    DestroyAfterCreate life;
    Vector3 targetPos;
    float direction = -1;
    float dashTimer = 2;
    // Start is called before the first frame update
    void Start()
    {
        localPosInit = transform.localPosition;
        Vector3 targetPos = GameObject.Find("Player").transform.position;
        life = GetComponent<DestroyAfterCreate>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 localPos = transform.position;
        dashTimer -= Time.deltaTime;
        if (dashTimer <= 0)
        {
            targetPos = GameObject.Find("Player").transform.position;
            dashTimer = 2;
        }

        transform.position = new Vector3(Mathf.Lerp(localPos.x, targetPos.x, 0.0125f), localPos.y, Mathf.Lerp(localPos.z, targetPos.z, 0.0125f));
    }
}
