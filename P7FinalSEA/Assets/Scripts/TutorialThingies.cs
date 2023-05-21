using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialThingies : MonoBehaviour
{
    public string tutorial;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject == GameObject.Find("Player"))
        {
            GameObject text = GameObject.Find("Player").GetComponent<PlayerControls>().tutorialText;
            text.SetActive(true);
            text.GetComponent<TMP_Text>().text = tutorial;
        }
    }
}
