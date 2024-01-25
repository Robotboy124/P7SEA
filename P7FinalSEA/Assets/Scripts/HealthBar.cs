using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public GameObject objectTracking;
    public TMP_Text healthPercent;
    float healthy;
    public Slider sliding;
    public Image fill;

    // Start is called before the first frame update
    void Start()
    {
        healthy = objectTracking.GetComponent<Damageable>().health;
        sliding.maxValue = healthy;
        sliding.value = 0;
        sliding.fillRect.gameObject.SetActive(false);
        UpdateValue(sliding.maxValue);
    }

    // Update is called once per frame
    public void UpdateValue(float damage)
    {
        sliding.fillRect.gameObject.SetActive(true);
        sliding.value = Mathf.Lerp(sliding.value, damage, 0.01f);
        if (healthPercent == null)
        {
            return;
        }
        else
        {
            healthPercent.text = Mathf.CeilToInt((damage/sliding.maxValue)*100) + "%";
        }
    }

    void Update()
    {
        if (objectTracking == null)
        {
            Destroy(gameObject);
        }
    }
}
