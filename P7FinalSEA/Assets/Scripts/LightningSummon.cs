using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSummon : MonoBehaviour
{
    public GameObject lightning;
    public float lightningTimer;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SummonLightning());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SummonLightning()
    {
        yield return new WaitForSeconds(lightningTimer);

        GameObject lights = Instantiate(lightning, transform.position + Vector3.up*1000, Quaternion.identity);
        lights.GetComponent<InstantiatedAttack>().ObjectUpdate(GetComponent<InstantiatedAttack>().objectSpawnedThis);
        lights.GetComponent<LightningDescend>().yLevel = transform.position.y;
        Destroy(gameObject);
    }
}
