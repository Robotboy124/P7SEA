using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingLightning : MonoBehaviour
{
    public GameObject groundLightning;
    public float initRotateSpeed;
    public float maxRotateSpeed;
    float rotateAccel;
    float currentRotateSpeed;

    private void Awake()
    {
        transform.LookAt(new Vector3(GameObject.Find("Player").transform.position.x + Random.Range(-5.0f, 5.0f), 0.5f, GameObject.Find("Player").transform.position.z + Random.Range(-5.0f, 5.0f)));
    }

    // Start is called before the first frame update
    void Start()
    {
        currentRotateSpeed = initRotateSpeed;
        StartCoroutine(XLightningSummon());
        rotateAccel = (maxRotateSpeed-initRotateSpeed)/(groundLightning.GetComponent<DestroyAfterCreate>().destroyTimer);
    }

    // Update is called once per frame
    void Update()
    {
        currentRotateSpeed += rotateAccel * Time.deltaTime;
        transform.Rotate(Vector3.up * currentRotateSpeed * Time.deltaTime);
    }

    IEnumerator XLightningSummon()
    {
        yield return new WaitForSeconds(1.5f);

        GameObject lightning = Instantiate(groundLightning, transform.position, Quaternion.identity);
        lightning.GetComponent<InstantiatedAttack>().ObjectUpdate(GetComponent<InstantiatedAttack>().objectSpawnedThis);

        yield return new WaitForSeconds(0.75f);

        for (int i = 1; i < 25; i++)
        {
            transform.Translate(Vector3.right * i * 4);
            GameObject lightningOne = Instantiate(groundLightning, transform.position, Quaternion.identity, gameObject.transform);
            lightningOne.transform.localPosition = Vector3.right * i * 4;
            lightningOne.GetComponent<InstantiatedAttack>().ObjectUpdate(GetComponent<InstantiatedAttack>().objectSpawnedThis);
            transform.Translate(Vector3.left * i * 8);
            GameObject lightningTwo = Instantiate(groundLightning, transform.position, Quaternion.identity, gameObject.transform);
            lightningTwo.GetComponent<InstantiatedAttack>().ObjectUpdate(GetComponent<InstantiatedAttack>().objectSpawnedThis);
            lightningTwo.transform.localPosition = Vector3.left * i * 4;
            transform.Translate(Vector3.right * i * 4);
            transform.Translate(Vector3.forward * i * 4);
            GameObject lightningThree = Instantiate(groundLightning, transform.position, Quaternion.identity, gameObject.transform);
            lightningThree.GetComponent<InstantiatedAttack>().ObjectUpdate(GetComponent<InstantiatedAttack>().objectSpawnedThis);
            lightningThree.transform.localPosition = Vector3.forward * i * 4;
            transform.Translate(Vector3.back * i * 8);
            GameObject lightningFour = Instantiate(groundLightning, transform.position, Quaternion.identity, gameObject.transform);
            lightningFour.GetComponent<InstantiatedAttack>().ObjectUpdate(GetComponent<InstantiatedAttack>().objectSpawnedThis);
            lightningFour.transform.localPosition = Vector3.back * i * 4;
            transform.Translate(Vector3.forward * i * 4);
        }
    }
}

