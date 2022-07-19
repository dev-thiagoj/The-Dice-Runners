using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTrap : MonoBehaviour
{   
    public Transform trap;
    public List<ParticleSystem> particleSystems;
    public List<Collider> colliders;
    public List<MeshRenderer> meshRenderers;

    public float timeToDestroy = 3;

    private void OnValidate()
    {
        if (trap == null) trap = GetComponent<Transform>();
    }

    [NaughtyAttributes.Button]
    public void HideTrap()
    {
        if (meshRenderers != null)
        {
            for (int i = 0; i < meshRenderers.Count; i++)
            {
                meshRenderers[i].enabled = false;
                colliders[i].enabled = false;
            }
        }

        if (particleSystems != null)
        {
            for (int j = 0; j < particleSystems.Count; j++)
            {
                particleSystems[j].Play();
            }
        }

        Destroy(gameObject, 3);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Dice"))
        {
            Debug.Log("Dice Collision");
            HideTrap();
        }
    }
}
