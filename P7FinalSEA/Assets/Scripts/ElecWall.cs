using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ElecWall : MonoBehaviour
{
    public float damage;
    BoxCollider col;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        col.center = new Vector3 (0, col.size.y/(-2f), 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == GameObject.Find("Player"))
        {
            other.gameObject.GetComponent<Damageable>().Damaged(damage);
            other.gameObject.transform.position = other.gameObject.GetComponent<PlayerControls>().checkpoint.position;
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.gameObject.GetComponent<PlayerControls>().respawning = true;
            other.gameObject.GetComponent<PlayerControls>().tutorialText.GetComponent<TMP_Text>().text = "Oh yeah, those hurt you.";
        }
        else if (other.gameObject.GetComponent<Current>() != null)
        {
            other.gameObject.transform.position = other.gameObject.GetComponent<Current>().transformStart;
        }
    }
}
