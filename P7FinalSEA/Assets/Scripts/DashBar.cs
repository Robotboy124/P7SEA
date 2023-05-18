using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashBar : MonoBehaviour
{
    public Slider dasher;
    public Image fill;

    // Start is called before the first frame update
    void Start()
    {
        dasher.maxValue = GameObject.Find("Player").GetComponent<PlayerControls>().maxDashStamina;
        dasher.value = dasher.maxValue;
        dasher.fillRect.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetValue(float dashValue)
    {
        dasher.fillRect.gameObject.SetActive(true);
        dasher.value = dashValue;
    }
}
