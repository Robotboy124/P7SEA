using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentProj : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Starter()
    {
        StartCoroutine (Translating());
    }

    IEnumerator Translating()
    {
        yield return new WaitForEndOfFrame();

        transform.Translate(Vector3.up * 25f * Time.deltaTime);
        StartCoroutine(Translating());
    }
}
