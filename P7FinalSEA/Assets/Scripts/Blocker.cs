using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocker : MonoBehaviour
{
    public GameObject[] walls;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WallSummon()
    {
        for(int i = 0; i < walls.Length; i++)
        {
            walls[i].SetActive(true);
        }
    }
}
