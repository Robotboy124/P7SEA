using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XLightning : MonoBehaviour
{

    public GameObject groundLightning;

    private void Awake()
    {
        transform.LookAt(new Vector3(GameObject.Find("Player").transform.position.x + Random.Range(-5.0f, 5.0f), 0.5f, GameObject.Find("Player").transform.position.z + Random.Range(-5.0f, 5.0f)));
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(XLightningSummon());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator XLightningSummon()
    {
        Instantiate(groundLightning, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(0.75f);

        for (int i = 1; i < 22; i++)
        {
            transform.Translate(Vector3.right*i*2);
            Instantiate(groundLightning, transform.position, Quaternion.identity);
            transform.Translate(Vector3.left*i*4);
            Instantiate(groundLightning, transform.position, Quaternion.identity);
            transform.Translate(Vector3.right*i*2);
            transform.Translate(Vector3.forward*i*2);
            Instantiate(groundLightning, transform.position, Quaternion.identity);
            transform.Translate(Vector3.back*i*4);
            Instantiate(groundLightning, transform.position, Quaternion.identity);
            transform.Translate(Vector3.forward*i*2);
        }

        Destroy(gameObject);
    }
}
