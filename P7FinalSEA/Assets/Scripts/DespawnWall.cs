using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnWall : MonoBehaviour
{
    public GameObject[] roomToCheck;
    public GameObject[] wallsToDespawn;
    float roomChecker;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < roomToCheck.Length; i++)
        {
            if (roomToCheck[i].activeInHierarchy == false)
            {
                roomChecker++;
            }
            else
            {
                roomChecker = 0;
            }
        }
        if (roomChecker >= roomToCheck.Length)
        {
            for(int u = 0; u < wallsToDespawn.Length; u++)
            {
                wallsToDespawn[u].SetActive(false);
            }
        }
    }
}
