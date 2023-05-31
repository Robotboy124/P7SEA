using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]

public class ParticleController : MonoBehaviour
{
    ParticleSystem particles;
    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
        var em = particles.emission;
        em.rateOverTime = particles.shape.scale.x * particles.shape.scale.y / 12;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
