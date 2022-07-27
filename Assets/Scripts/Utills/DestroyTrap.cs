using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTrap : MonoBehaviour
{   
    public Transform trap;
    public ParticleSystem[] particleSystems;
    public Collider[] colliders;
    //public List<MeshRenderer> meshRenderers;
    public MeshRenderer[] meshRenderers;
    public SkinnedMeshRenderer[] skinnedRenderers;

    [Header("SFX")]
    public AudioSource audioSource;
    [Range (0,1)]
    public float volumeSFX;

    public float timeToDestroy = 3;

    private void OnValidate()
    {
        if (trap == null) trap = GetComponent<Transform>();
        if (meshRenderers == null) meshRenderers = GetComponentsInChildren<MeshRenderer>();
        //particleSystems = GetComponentsInChildren<ParticleSystem>();
        colliders = GetComponentsInChildren<Collider>();
        skinnedRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    private void Awake()
    {
        if (audioSource == null) return;
    }

    public void HideTrap()
    {
        if (meshRenderers != null)
        {
            foreach (var renderer in meshRenderers)
            {
                renderer.enabled = false;
            }
        }

        if(skinnedRenderers != null)
        {
            foreach (var skinned in skinnedRenderers)
            {
                skinned.enabled = false;
            }
        }

        if(colliders != null)
        {
            foreach (var collider in colliders)
            {
                collider.enabled = false;
            }
        }

        if(particleSystems != null)
        {
            foreach (var ps in particleSystems)
            {
                ps.Play();
            }
        }

        if(audioSource != null)
        {
            audioSource.volume = volumeSFX;
            audioSource.Play();
        }

        Destroy(gameObject, 3);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Dice"))
        {
            HideTrap();
        }
    }
}
