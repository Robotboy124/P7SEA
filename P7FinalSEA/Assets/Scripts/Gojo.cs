using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gojo : MonoBehaviour
{
    public GameObject player;
    public float neutralWalkSpeed;
    Rigidbody rb;
    float walkTimer = 1.0f;
    float walkDir = 1.0f;
    bool checkingForWall = true;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Walking());
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        WalkCycle();
    }

    void WalkCycle()
    {
        transform.Translate(Vector3.right * walkDir * neutralWalkSpeed * Time.deltaTime);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(rb.velocity + transform.position), out hit, rb.velocity.magnitude))
        {
            if (hit.collider.gameObject.CompareTag("ElecWall"))
            {
                walkDir *= -1;
            }
        }
    }

    IEnumerator Walking()
    {
        yield return new WaitForSeconds(walkTimer);

        if (Random.Range(0f, 100f) < 25f)
        {
            walkDir *= -1;
        }

        StartCoroutine(Walking());
    }
}
