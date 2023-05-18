using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadTimer : MonoBehaviour
{
    public Slider reloader;
    public Image fill;
    // Start is called before the first frame update
    void Start()
    {
        reloader.maxValue = GameObject.Find("Player").GetComponent<PlayerControls>().fireRateInitial;
        reloader.value = reloader.maxValue;
        reloader.fillRect.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetValue(float maxReload, float currentReload)
    {
        reloader.fillRect.gameObject.SetActive(true);
        reloader.maxValue = maxReload;
        reloader.value = maxReload - currentReload;
        if (reloader.value == reloader.maxValue)
        {
            fill.color = Color.green;
        }
        else if (reloader.value < reloader.maxValue)
        {
            fill.color = Color.red;
        }
    }
}
