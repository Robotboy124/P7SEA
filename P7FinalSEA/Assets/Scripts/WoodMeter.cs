using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WoodMeter : MonoBehaviour
{
    public Slider sliding;
    public Image fill;
    // Start is called before the first frame update
    void Start()
    {
        sliding.maxValue = 4;
        sliding.value = sliding.maxValue;
        sliding.fillRect.gameObject.SetActive(false);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void UpdateValue(float currentParry)
    {
        sliding.fillRect.gameObject.SetActive(true);
        sliding.value = currentParry;
    }
}
