using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFieldManager : MonoBehaviour
{
    [SerializeField] ParticleSystem particles;

    private void OnValidate()
    {   
        if (particles == null) particles = GetComponentInChildren<ParticleSystem>();
    }

    public void StartParticleField()
    {
        particles.Play();
    }
}
