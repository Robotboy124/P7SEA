using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAfterWave : MonoBehaviour
{
    public GameObject[] waveCheck;
    float waveNumber;
    public GameObject[] nextWave;
    public GameObject spawnParticle;
    public float spawnCheck;
    // Start is called before the first frame update
    void Start()
    {
        waveNumber = waveCheck.Length;
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < waveCheck.Length; i++)
        {
            if (waveCheck[i].activeInHierarchy == false)
            {
                waveNumber++;
            }
            else
            {
                waveNumber = 0;
            }
        }

        if (waveNumber < waveCheck.Length)
        {
            waveNumber = 0;
        }
        else if (waveNumber == waveCheck.Length && spawnCheck == 0)
        {
            for (int i = 0; i < nextWave.Length; i++)
            {
                nextWave[i].SetActive(true);
                Instantiate(spawnParticle, nextWave[i].transform.position, Quaternion.identity);
            }
            spawnCheck++;
        }
    }
}
