using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSummon : MonoBehaviour
{
    public GameObject lightning;
    public bool blueLightning;
    public float lightningTimer;
    public float damageMulti = 1;

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

        if (blueLightning)
        {
            GameObject lights = Instantiate(lightning, transform.position + Vector3.up * 800, Quaternion.identity);
            lights.GetComponent<InstantiatedAttack>().ObjectUpdate(GetComponent<InstantiatedAttack>().objectSpawnedThis);
            lights.GetComponent<DamageFieldStay>().damage *= damageMulti;
            lights.GetComponent<LightningDescend>().yLevel = transform.position.y;
            Destroy(gameObject);
        }
        else
        {
            GameObject lights = Instantiate(lightning, transform.position + Vector3.up * 800, Quaternion.identity);
            lights.GetComponent<InstantiatedAttack>().ObjectUpdate(GetComponent<InstantiatedAttack>().objectSpawnedThis);
            lights.GetComponent<DamageField>().damage *= damageMulti;
            lights.GetComponent<LightningDescend>().yLevel = transform.position.y;
            Destroy(gameObject);
        }
    }
}
