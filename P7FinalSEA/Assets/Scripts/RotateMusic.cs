using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMusic : MonoBehaviour
{
    public AudioClip[] phaseMusic;
    public AudioSource speakers;
    public Damageable health;
    // Start is called before the first frame update
    void Start()
    {
        speakers.clip = phaseMusic[0];
        speakers.Play();
        health = GetComponent<Damageable>();
    }

    // Update is called once per frame
    void Update()
    {
        speakers.loop = true;
        if (health.health <= health.initialHealth * 0.6f && health.health > health.initialHealth * 0.2f)
        {
            speakers.clip = phaseMusic[1];
            StartCoroutine(OneFramePlay());
        }
        else if (health.health <= health.initialHealth * 0.2f)
        {
            speakers.clip = phaseMusic[2];
            StartCoroutine(OneFramePlay());
        }
        else if (health.health > health.initialHealth * 0.6f)
        {
            speakers.clip = phaseMusic[0];
            StartCoroutine(OneFramePlay());
        }
    }

    IEnumerator OneFramePlay()
    {
        speakers.Pause();
        speakers.Play();
        yield return new WaitForEndOfFrame();
    }
}
