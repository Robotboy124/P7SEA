using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatedAttack : MonoBehaviour
{
    public GameObject objectSpawnedThis;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ObjectUpdate(GameObject thiss)
    {
        objectSpawnedThis = thiss;
    }
}
